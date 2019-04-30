using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dms.Product.WebApplication.Models;
using System.Timers;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Dms.Product.Respository.IService;
using Dms.Product.General.Tool;
using Dms.Product.Base.Model;
using Dms.Product.Respository.Model;
using Dms.Prodoct.Domain.DataServce;

namespace Dms.Product.WebApplication.Controllers
{

    /// <summary> 监控配置控制器 </summary>
    public class MonitorController : ControllerBase
    {

        IMonitorSetRespository _respository;

        public MonitorController(IMonitorSetRespository respository)
        {
            _respository = respository;
        }

        /// <summary> 转到监控列表 </summary>
        public async Task<IActionResult> Index()
        {
            var collection = await _respository.GetMonitorList();

            return PartialView(collection);
        }

        /// <summary> 转到历史列表 </summary>
        public async Task<IActionResult> History()
        {
            await this.RefreshSeletMonitorDropList(l => true);

            return View();
        }

        /// <summary> 转到历史列表 </summary>
        public async Task<IActionResult> Alarm()
        {
            //await this.RefreshDropList(l => true, l => true);

            await this.RefreshSeletMonitorDropList(l => true);

            var alarmtypes = await _respository.GetAlarmTypes();

            ViewBag.AlarmTypeList = new SelectList(alarmtypes, "ID", "NAME");

            return View();
        }

        [HttpPost]
        [Route("Monitor/RefreshAlarmData")]
        public PartialViewResult RefreshAlarmData(string id, string date, string typeid)
        {

            DateTime dateTime = date.ToDateTime("MM/dd/yyyy");

            var collection = _respository.GetAlarms(id == "-全部-" ? null : id, dateTime, typeid == "-全部-" ? null : typeid);

            return PartialView("_AlarmView", collection.Result == null ? new List<AlarmViewModel>() : collection.Result);

        }

        [HttpPost]
        [Route("Monitor/RefreshHistoryData")]
        public PartialViewResult RefreshHistoryData(string id, string date)
        {

            //DateTime dateTime = date.ToDateTime("MM/dd/yyyy");

            if(string .IsNullOrEmpty(id))
            {
                return null;
            }

           var monitor= this._respository.GetMonitorByID(id);

            DateTime dateTime = string.IsNullOrEmpty(date) ? DateTime.Now.Date : date.ToDateTime("MM/dd/yyyy");

            string matid = monitor.Result==null?null: monitor.Result.Bed?.MATID;

            var collection = _respository.GetHistoryList(matid, dateTime.ToDateTimeString(), dateTime.AddDays(1).ToDateTimeString());

            return PartialView("_HistoryView", Tuple.Create(monitor.Result, collection.Result)); 

        }

        [HttpPost]
        [Route("Monitor/RefreshLocalHistoryData")]
        public PartialViewResult RefreshLocalHistoryData(string id, string date)
        {

            DateTime dateTime = string.IsNullOrEmpty(date) ? DateTime.Now : date.ToDateTime("MM/dd/yyyy").AddDays(1);

            var collection = _respository.GetLocalHistoryList(id, dateTime.AddDays(-1).ToDateTimeString(), dateTime.ToDateTimeString());

            var monitorViewModel = this._respository.GetMonitorByID(id);

            return PartialView("_HistoryView", Tuple.Create(monitorViewModel.Result, collection.Result == null ? new List<HistoryDataViewModel>() : collection.Result));

        }



        // POST: Monitor/Delete/5
        [HttpPost, ActionName("DeleteAlarm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAlarm(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _respository.DeleteAlarm(id);

            await _respository.WriteUserLogger(this.GetUserID(), UserLoggerConfiger.DeleteAlarm, id);

            return RedirectToAction(nameof(Alarm));

        }


        /// <summary> 监控列表 </summary>
        public async Task<IActionResult> Monitor()
        {
            //var collection = await _respository.GetMonitorList();

            //UserConfigViewModel model = new UserConfigViewModel();

            //_configer.UseVoice = true;

            var model = await this._respository.GetUserConfiger(l => l.ID == this.GetUserID());

            return View(model);

        }

        /// <summary> 监控图标分布视图 </summary>
        public async Task<PartialViewResult> MonitorCenter()
        {
            var collection = await _respository.GetMonitorList();

            return PartialView("_MonitorCenter", collection);
        }

        /// <summary>
        /// 局部刷新监控列表分布视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult RefreshMonitor(string type)
        {
            //var result = _respository.GetMonitorList();

            //var result = _respository.GetMonitorListTest();

            var result = _respository.GetLocalRealList();


           System.Diagnostics.Debug.WriteLine("RefreshMonitor");


            type = string.IsNullOrEmpty(type) ? "Type1" : type;

            return PartialView("_MonitorCenter", Tuple.Create(result.Result, type));

        }

        /// <summary>
        /// 局部刷新监控人员信息分布视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ShowCustomer(string id)
        {
            if (!this.CHeckSession())
            {
                _respository.LogInfo("CHeckSession登录超时");
                return null;
            }

            var result = _respository.GetCurstoms();

            var find = result.Result.Where(l => l.ID == id).FirstOrDefault();

            return PartialView("_CustomerView", find);
        }

        /// <summary>
        /// 局部刷新监控人员信息分布视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ShowRealLine(string id)
        {
            if (!this.CHeckSession())
            {
                _respository.LogInfo("CHeckSession登录超时");
                return null;
            }

            var reult = this._respository.GetMonitorByID(id);

            return PartialView("_RealLineView", reult.Result);
        }

        [HttpPost]
        [Route("Monitor/RefreshRealLine")]
        public JsonResult RefreshRealLine(string monitorID)
        {
            if (string.IsNullOrEmpty(monitorID)) return null;


           System.Diagnostics.Debug.WriteLine("RefreshRealLine");


            List<Tuple<string, string, string>> result = _respository.GetRealLine(monitorID).Result;

            Func<Tuple<string, string, string>, string> convertxAxis = l =>
            {
                return l.Item1.Split(' ').Count() > 1 ? l.Item1.Split(' ')[1] : l.Item1;
            };

            List<ReportConvertEngine<Tuple<string, string, string>, int?>> matchs = new List<ReportConvertEngine<Tuple<string, string, string>, int?>>();


            ReportConvertEngine<Tuple<string, string, string>, int?> engine1 = new ReportConvertEngine<Tuple<string, string, string>, int?>();

            engine1.Convert = l =>
            {
                return l.Item2.ToIntNull();
            };

            engine1.MatchValue = l => l.HasValue;

            engine1.Name = "心率";

            engine1.Type = "line";

            matchs.Add(engine1);

            ReportConvertEngine<Tuple<string, string, string>, int?> engine2 = new ReportConvertEngine<Tuple<string, string, string>, int?>();

            engine2.Convert = l =>
            {
                return l.Item3.ToIntNull();
            };

            engine2.MatchValue = l => l.HasValue;

            engine2.Name = "呼吸";

            engine2.Type = "line";

            matchs.Add(engine2);


            string realmessage = _respository.GetRealMessage(monitorID).Result;

            var series = ToolService.Instance.Create(result.ToList(), convertxAxis, matchs.ToList());

            dynamic data = new
            {
                d = new { data = series },
                m = new { message = realmessage }
            };

            return Json(data);
        }

        [HttpPost]
        [Route("Monitor/Details/RefreshDetailRealLine")]
        public JsonResult RefreshDetailRealLine(string monitorID)
        {
            //var result = _respository.GetRealLineTest().Result; ;

            //var json = ToolService.Instance.GroupByExpression(result,
            //    l => l.ORGNAME,
            //    l => l.Max(k => k.CDTOTAL),
            //    l => l.Min(k => k.CDTOTAL),
            //    l => l.Average(k => k.CDTOTAL),
            //    l => l.Count());

            ////var json = DataService.Instance.GroupByExpression(result,
            ////   l => l.ORGNAME,
            ////   l => l.Max(k => k.CDTOTAL),
            ////   l => l.Min(k => k.CDTOTAL),
            ////   l => l.Average(k => k.CDTOTAL),
            ////   l => l.Min(k => k.OLDTOTAL), l => l.Count());

            var result = _respository.GetRealLineTest().Result;

            Func<jw_add_data, string> convertxAxis = l =>
            {
                return l.UDATE.Value.ToString("yyyy-MM-dd");
            };

            Predicate<PropertyInfo> except = l =>
            {
                return l.Name.ToUpper() == "FILETOTAL"
                || l.Name.ToUpper() == "OLDTOTAL"
                || l.Name.ToUpper() == "PERFECTTOTLE"
                || l.Name.ToUpper() == "COUNT"
                || l.Name.ToUpper() == "WOMANRATE"
                || l.Name.ToUpper() == "CDTOTAL"
                || l.Name.ToUpper() == "CHILDRATE"
                || l.Name.ToUpper() == "COUNT"
                || l.Name.ToUpper() == "YEAR"
                || l.Name.ToUpper() == "TYPE";
            };

            List<ReportConvertEngine<jw_add_data, int?>> matchs = new List<ReportConvertEngine<jw_add_data, int?>>();

            var vaildIntCollection = typeof(jw_add_data).GetProperties().ToList().FindAll(l => l.PropertyType == typeof(int?) && (l.Name == "PERFECTTOTLE" || l.Name == "OLDTOTAL"));

            foreach (var item in vaildIntCollection)
            {
                //if (except(item)) continue;

                ReportConvertEngine<jw_add_data, int?> engine = new ReportConvertEngine<jw_add_data, int?>();

                engine.Convert = l =>
                {
                    return (int?)item.GetValue(l);
                };

                engine.MatchValue = l => l.HasValue;

                DisplayAttribute display = item.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;

                engine.Name = display == null ? item.Name : display.Name;

                engine.Type = "line";

                matchs.Add(engine);
            }

            //convertToValueList.Add(l => l.CDTOTAL);
            //convertToValueList.Add(l => l.CHILDRATE);
            //convertToValueList.Add(l => l.COUNT);
            //convertToValueList.Add(l => l.CYTOTAL);
            //convertToValueList.Add(l => l.DBTOTAL);

            System.Diagnostics.Debug.WriteLine(result.Count());

            var series = ToolService.Instance.Create(result.ToList(), convertxAxis, matchs.Take(2).ToList());

            return Json(series);
        }

        [HttpPost]
        [Route("Monitor/RefreshRealLine_test")]
        public JsonResult RefreshRealLine_test(string monitorID)
        {
            //var result = _respository.GetRealLineTest().Result; ;

            //var json = ToolService.Instance.GroupByExpression(result,
            //    l => l.ORGNAME,
            //    l => l.Max(k => k.CDTOTAL),
            //    l => l.Min(k => k.CDTOTAL),
            //    l => l.Average(k => k.CDTOTAL),
            //    l => l.Count());

            ////var json = DataService.Instance.GroupByExpression(result,
            ////   l => l.ORGNAME,
            ////   l => l.Max(k => k.CDTOTAL),
            ////   l => l.Min(k => k.CDTOTAL),
            ////   l => l.Average(k => k.CDTOTAL),
            ////   l => l.Min(k => k.OLDTOTAL), l => l.Count());

            var result = _respository.GetRealLineTest().Result;

            Func<jw_add_data, string> convertxAxis = l =>
            {
                return l.UDATE.Value.ToString("yyyy-MM-dd");
            };

            Predicate<PropertyInfo> except = l =>
            {
                return l.Name.ToUpper() == "FILETOTAL"
                || l.Name.ToUpper() == "OLDTOTAL"
                || l.Name.ToUpper() == "PERFECTTOTLE"
                || l.Name.ToUpper() == "COUNT"
                || l.Name.ToUpper() == "WOMANRATE"
                || l.Name.ToUpper() == "CDTOTAL"
                || l.Name.ToUpper() == "CHILDRATE"
                || l.Name.ToUpper() == "COUNT"
                || l.Name.ToUpper() == "YEAR"
                || l.Name.ToUpper() == "TYPE";
            };

            List<ReportConvertEngine<jw_add_data, int?>> matchs = new List<ReportConvertEngine<jw_add_data, int?>>();

            var vaildIntCollection = typeof(jw_add_data).GetProperties().ToList().FindAll(l => l.PropertyType == typeof(int?) && (l.Name == "PERFECTTOTLE" || l.Name == "OLDTOTAL"));

            foreach (var item in vaildIntCollection)
            {
                //if (except(item)) continue;

                ReportConvertEngine<jw_add_data, int?> engine = new ReportConvertEngine<jw_add_data, int?>();

                engine.Convert = l =>
                {
                    return (int?)item.GetValue(l);
                };

                engine.MatchValue = l => l.HasValue;

                DisplayAttribute display = item.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;

                engine.Name = display == null ? item.Name : display.Name;

                engine.Type = "line";

                matchs.Add(engine);
            }

            //convertToValueList.Add(l => l.CDTOTAL);
            //convertToValueList.Add(l => l.CHILDRATE);
            //convertToValueList.Add(l => l.COUNT);
            //convertToValueList.Add(l => l.CYTOTAL);
            //convertToValueList.Add(l => l.DBTOTAL);

            System.Diagnostics.Debug.WriteLine(result.Count());

            var series = ToolService.Instance.Create(result.ToList(), convertxAxis, matchs.Take(2).ToList());

            dynamic data = new
            {
                d = new { data = series },
                m = new { message = "刚开始就赶快大结局" }
            };

            return Json(data);
        }

        [HttpPost]
        [Route("Monitor/GetCustomer")]
        [Route("Monitor/Edit/GetCustomer")]
        public JsonResult GetCustomer(string id)
        {
            var customers = _respository.GetCurstoms().Result;

            var result = customers.Where(l => l.ID == id).FirstOrDefault();

            return Json(result);
        }

        [HttpPost]
        [Route("Monitor/RefreshTurnCustom")]
        public JsonResult RefreshTurnCustom(string id)
        {
            _respository.RefreshTurnCustom(id);

            return Json(null);
        }

        [HttpPost]
        [Route("Monitor/SaveVoiceChecked")]
        public JsonResult SaveVoiceChecked(bool value)
        {
            _respository.SaveUser(l => l.ID == this.GetUserID(), l => l.USEVOICE = value ? 1 : 0);

            return Json(null);
        }


        [HttpPost]
        [Route("Monitor/GetMat")]
        public JsonResult GetMat(string id)
        {
            var bed = _respository.GetBeds().Result.Where(l => l.ID == id).FirstOrDefault();

            //var result = _respository.GetMats().Result.Where(l => l.CODE == beds.MATID).FirstOrDefault();

            //var result = _respository.GetMats().Result.Where(l => l.CODE == beds.MATID).FirstOrDefault();

            return Json(bed.MATID);
        }

        public async Task<IActionResult> Details(string id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var jCSJ_MONITOR = await _context.Moniters.FirstOrDefaultAsync(m => m.ID == id);
            //if (jCSJ_MONITOR == null)
            //{
            //    return NotFound();
            //}

            var reult = await this._respository.GetMonitorByID(id);

            return View(reult);
        }

        /// <summary>
        /// 新建监控
        /// </summary>
        // GET: Monitor/Create
        public async Task<IActionResult> Create()
        {
            var monitors = await this._respository.GetListAsync();

            Func<ehc_dv_customer, bool> custom = l =>
            {
                return !monitors.Exists(k => k.CUSTOMID == l.ID);
            };

            Func<ehc_dv_bed, bool> bed = l =>
            {
                return !monitors.Exists(k => k.BEDID == l.ID);
            };

            await this.RefreshDropList(custom, bed);

            return View();
        }


        /// <summary>
        /// 新建监控提交
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(MonitorItemViewModel viewModel)
        {

            //if (ModelState.IsValid)
            //{
            //    await _respository.Create(viewModel);

            //    await _respository.WriteUserLogger(this.GetUserID(), "添加监控", viewModel.Monitor.ID);

            //    return RedirectToAction(nameof(Index));
            //}

            if (viewModel.IsFormVisble())
            {
                await _respository.Create(viewModel);

                await _respository.WriteUserLogger(this.GetUserID(), UserLoggerConfiger.AddMonitor, viewModel.Monitor.ID);

                return RedirectToAction(nameof(Index));
            }

            var monitors = await this._respository.GetListAsync();

            Func<ehc_dv_customer, bool> custom = l =>
            {
                return !monitors.Exists(k => k.CUSTOMID == l.ID);
            };

            Func<ehc_dv_bed, bool> bed = l =>
            {
                return !monitors.Exists(k => k.BEDID == l.ID);
            };

            await this.RefreshDropList(custom, bed);

            return View(viewModel);
        }

        // GET: Monitor/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _respository.GetMonitorDeitalByID(id);

            if (result == null)
            {
                return NotFound();
            }

            var monitors = await this._respository.GetListAsync();

            Func<ehc_dv_customer, bool> custom = l =>
              {
                  return (!monitors.Exists(k => k.CUSTOMID == l.ID)) || l.ID == result.Monitor.CUSTOMID;
              };

            Func<ehc_dv_bed, bool> bed = l =>
            {
                return (!monitors.Exists(k => k.BEDID == l.ID)) || l.ID == result.Monitor.BEDID;
            };

            await this.RefreshDropList(custom, bed);

            return View(result);
        }

        /// <summary> 刷新新建下拉列表 </summary>
        async Task RefreshDropList(Func<ehc_dv_customer, bool> canCanstom, Func<ehc_dv_bed, bool> canBed)
        {
            var monitors = await this._respository.GetListAsync();

            var customs = await _respository.GetCurstoms();

            customs = customs.Where(canCanstom);

            //var mats = await _respository.GetMats();
            var beds = await _respository.GetBeds();

            beds = beds.Where(canBed);

            ViewBag.CustomerList = new SelectList(customs, "ID", "NAME");

            //ViewBag.MatList = new SelectList(mats, "ID", "NAME");

            ViewBag.BedList = new SelectList(beds, "ID", "NAME");
        }

        /// <summary> 刷新新建下拉列表 </summary>
        async Task RefreshSeletMonitorDropList(Func<ehc_dv_monitor, bool> canMonitor)
        {
            var monitors = await this._respository.GetListAsync(l => canMonitor(l));

            var customs = await _respository.GetCurstoms(); 

            var beds = await _respository.GetBeds();

            Func<ehc_dv_monitor, string> converto = l =>
               {
                   var custom = customs.FirstOrDefault(k => k.ID == l.CUSTOMID);
                   var bed = beds.FirstOrDefault(k => k.ID == l.BEDID);

                   return $"[{beds.FirstOrDefault(k => k.ID == l.BEDID)?.CODE}]  {custom?.NAME} {custom?.CARDID} {custom?.SEX} {custom?.AGE}";
               };

            var monitorlist = monitors.Select(l => Tuple.Create(l.ID, converto(l)));

            ViewBag.Monitors = new SelectList(monitorlist, "Item1", "Item2");
        }


        // POST: Monitor/Edit/5 
        [HttpPost]
        public async Task<IActionResult> Edit(string id, MonitorItemViewModel viewModel)
        {
            if (id != viewModel.Monitor.ID.ToString())
            {
                return NotFound();
            }

            if (viewModel.IsFormVisble())
            {
                try
                {
                    await _respository.Edit(viewModel);

                    await _respository.WriteUserLogger(this.GetUserID(), UserLoggerConfiger.EditMonitor, viewModel.Monitor.CUSTOMID);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_respository.Exist(id).Result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                } 
                
            }

            var result = await _respository.GetMonitorDeitalByID(id);

            if (result == null)
            {
                return NotFound();
            }

            var monitors = await this._respository.GetListAsync();

            Func<ehc_dv_customer, bool> custom = l =>
            {
                return (!monitors.Exists(k => k.CUSTOMID == l.ID)) || l.ID == result.Monitor.CUSTOMID;
            };

            Func<ehc_dv_bed, bool> bed = l =>
            {
                return (!monitors.Exists(k => k.BEDID == l.ID)) || l.ID == result.Monitor.BEDID;
            };

            await this.RefreshDropList(custom, bed);

            return View(viewModel);
        }

        // POST: Monitor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _respository.Delete(id);

            await _respository.WriteUserLogger(this.GetUserID(), UserLoggerConfiger.DeleteMonitor, id);

            return RedirectToAction(nameof(Index));

        }
    }
}
