using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Dms.Prodoct.Domain.DataServce;
using Dms.Product.Base.Model;
using Dms.Product.General.Tool;
using Dms.Product.Respository.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dms.Product.WebApplication.Controllers
{
    public class AnalyticalController : ControllerBase
    {
        IMonitorSetRespository _respository;

        public AnalyticalController(IMonitorSetRespository respository)
        {
            _respository = respository;
        }
        public IActionResult Index()
        {
            return View();
        }


        /// <summary> 转到历史列表 </summary>
        public async Task<IActionResult> Analytical()
        {
            await this.RefreshSeletMonitorDropList(l => true);

            return View();
        }

        /// <summary> 转到历史列表 </summary>
        public async Task<IActionResult> SleepQuality(string id)
        {
            await this.RefreshSeletMonitorDropList(l => true);
            return View();
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

                return $"{custom?.NAME} [{beds.FirstOrDefault(k => k.ID == l.BEDID)?.CODE}] {custom?.CARDID} {custom?.SEX} {custom?.AGE}";
            };

            var monitorlist = monitors.Select(l => Tuple.Create(l.ID, converto(l)));

            ViewBag.Monitors = new SelectList(monitorlist, "Item1", "Item2");
        }



        /// <summary>
        /// 刷新分布视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult RefreshAnalytical(string id, string date)
        {
            DateTime dateTime = date.ToDateTime("MM/dd/yyyy");

            var monitorViewModel = this._respository.GetMonitorByID(id);

            return PartialView("_AnalyticalView", monitorViewModel.Result);
        }

        /// <summary>
        /// 刷新分布视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult RefreshSleepQuality(string id, string date)
        {

            var monitorViewModel = this._respository.GetMonitorByID(id);

            return PartialView("_SleepQualityView", monitorViewModel.Result);

        }

        [HttpPost]
        [Route("Analytical/GetAnalyticalData")]
        public JsonResult GetAnalyticalData(string id, string date)
        {
            var monitor = this._respository.GetMonitorByID(id).Result;

            string matid = monitor?.Bed?.MATID;

            DateTime dateTime = date.ToDateTime("MM/dd/yyyy");
            string start = dateTime.AddHours(8).ToDateTimeString();
            string end = dateTime.AddHours(20).ToDateTimeString();

            var result1 = _respository.GetHistoryList(matid, start, end).Result;

            string end1 = dateTime.AddHours(32).ToDateTimeString();
            var result2 = _respository.GetHistoryList(matid, end, end1).Result;

            Func<ehc_dd_data, string> convertxAxis = l =>
            {
                return l.createTime.ToDateTime("yyyy-MM-ddTHH:mm:ss").ToString("HH:mm:ss");
            };

            List<ReportConvertEngine<ehc_dd_data, int?>> matchs = new List<ReportConvertEngine<ehc_dd_data, int?>>();

            var vaildIntCollection = typeof(ehc_dd_data).GetProperties().ToList().FindAll(l => l.Name == "heartBeat" || l.Name == "breath");

            foreach (var item in vaildIntCollection)
            {

                ReportConvertEngine<ehc_dd_data, int?> engine = new ReportConvertEngine<ehc_dd_data, int?>();

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

            var series1 = ToolService.Instance.Create(result1?.data?.OrderBy(l=>l.createTime).ToList(), convertxAxis, matchs.ToList());
            var series2 = ToolService.Instance.Create(result2?.data?.OrderBy(l => l.createTime).ToList(), convertxAxis, matchs.ToList());


            string message1 = dateTime.ToString("yyyy-MM-dd") + $"  08:00-20:00 日间 <small>心率：<strong>{ result1?.result?.heartBeat.max}</strong>/<strong>{ result1?.result?.heartBeat.min}</strong> 呼吸：<strong>{ result1?.result?.breath.max}</strong>/<strong>{ result1?.result?.breath.min}</strong>离床次数：<strong>{ result1?.result?.offBed?.times}</strong>次 体动次数：<strong>{ result1?.result?.move?.times}</strong>次 </ small >";
            string message2 = dateTime.ToString("yyyy-MM-dd") + $"  20:00-08:00 夜间 <small>心率：<strong>{ result2?.result?.heartBeat.max}</strong>/<strong>{ result2?.result?.heartBeat.min}</strong> 呼吸：<strong>{ result2?.result?.breath.max}</strong>/<strong>{ result2?.result?.breath.min}</strong>离床次数：<strong>{ result2?.result?.offBed?.times}</strong>次 体动次数：<strong>{ result2?.result?.move?.times}</strong>次 </ small >";

            dynamic data = new
            {
                d1 = new { data = series1, message = message1 },
                d2 = new { data = series2, message = message2 }
            };

            return Json(data);
        }

        [HttpPost]
        [Route("Analytical/GetSleepQualityData")]
        public JsonResult GetSleepQualityData(string id, string date)
        {
            var monitor = this._respository.GetMonitorByID(id).Result;

            string matid = monitor?.Bed?.MATID;

            DateTime dateTime = date.ToDateTime("MM/dd/yyyy");
            string start = dateTime.AddHours(8).ToDateTimeString();
            string end = dateTime.AddHours(20).ToDateTimeString();

            var result1 = _respository.GetHistoryList(matid, start, end).Result;
            string end1 = dateTime.AddHours(32).ToDateTimeString();
            var result2 = _respository.GetHistoryList(matid, end, end1).Result;


            List<Tuple<SleepItem, int>> collection1 = new List<Tuple<SleepItem, int>>();

            if (result1?.result?.sleepResult?.GoToSleep?.sleepRecords != null)
            {
                var convert = result1?.result?.sleepResult?.GoToSleep?.sleepRecords.Select(l => Tuple.Create(l, 1));

                collection1.AddRange(convert);
            }

            if (result1?.result?.sleepResult?.LightSleep?.sleepRecords != null)
            {
                var convert = result1?.result?.sleepResult?.LightSleep?.sleepRecords.Select(l => Tuple.Create(l, 1));

                collection1.AddRange(convert);
            }

            if (result1?.result?.sleepResult?.DeepSleep?.sleepRecords != null)
            {
                var convert = result1?.result?.sleepResult?.DeepSleep?.sleepRecords.Select(l => Tuple.Create(l, 1));

                collection1.AddRange(convert);
            }

            collection1 = collection1.OrderBy(l => l.Item1.StartTime).ToList();

          

            List<Tuple<SleepItem, int>> collection2 = new List<Tuple<SleepItem, int>>();

            if (result2?.result?.sleepResult?.GoToSleep?.sleepRecords != null)
            {
                var convert = result2?.result?.sleepResult?.GoToSleep?.sleepRecords.Select(l => Tuple.Create(l, 1));

                collection2.AddRange(convert);
            }

            if (result2?.result?.sleepResult?.LightSleep?.sleepRecords != null)
            {
                var convert = result2?.result?.sleepResult?.LightSleep?.sleepRecords.Select(l => Tuple.Create(l, 1));

                collection2.AddRange(convert);
            }

            if (result2?.result?.sleepResult?.DeepSleep?.sleepRecords != null)
            {
                var convert = result2?.result?.sleepResult?.DeepSleep?.sleepRecords.Select(l => Tuple.Create(l, 1));

                collection2.AddRange(convert);
            }

           

            //if (collection1.Count == 0)
            //{
            //    collection1.Add(Tuple.Create(new SleepItem() { StartTime = DateTime.Now.Date.AddHours(8), EndTime = DateTime.Now.Date.AddHours(8) }, 0));

            //    collection1.Add(Tuple.Create(new SleepItem() { StartTime = DateTime.Now.Date.AddHours(20), EndTime = DateTime.Now.Date.AddHours(20) }, 0));
            //}

            //if (collection2.Count == 0)
            //{
            //    collection2.Add(Tuple.Create(new SleepItem() { StartTime = DateTime.Now.Date.AddHours(20), EndTime = DateTime.Now.Date.AddHours(20) }, 0));

            //    collection2.Add(Tuple.Create(new SleepItem() { StartTime = DateTime.Now.Date.AddHours(32), EndTime = DateTime.Now.Date.AddHours(32) }, 0));
            //}

            collection2 = collection2.OrderBy(l => l.Item1.StartTime).ToList();

            Func<Tuple<SleepItem, int>, string> convertxAxis = l =>
            {
                return l.Item1.StartTime.ToString("HH:mm:ss");
            };

            List<ReportConvertEngine<Tuple<SleepItem, int>, int?>> matchs = new List<ReportConvertEngine<Tuple<SleepItem, int>, int?>>();



            ReportConvertEngine<Tuple<SleepItem, int>, int?> engine = new ReportConvertEngine<Tuple<SleepItem, int>, int?>();

            engine.Convert = l =>
            {
                return l.Item2;
            };

            engine.MatchValue = l => l.HasValue;

            //DisplayAttribute display = item.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;

            //engine.Name = display == null ? item.Name : display.Name;

            engine.Name = "睡眠状态";

            engine.Type = "bar";

            matchs.Add(engine);


            var series1 = ToolService.Instance.Create(collection1, convertxAxis, matchs.ToList());
            var series2 = ToolService.Instance.Create(collection2, convertxAxis, matchs.ToList());


            string message1 = dateTime.ToString("yyyy-MM-dd") + $"  08:00-20:00 日间 <small>睡眠时间：<strong>{ result1?.result?.sleepTime}</strong> </ small >";
            string message2 = dateTime.ToString("yyyy-MM-dd") + $"  20:00-08:00 夜间 <small>睡眠时间：<strong>{ result2?.result?.sleepTime}</strong> </ small >";

            dynamic data = new
            {
                d1 = new { data = series1, message = message1 },
                d2 = new { data = series2, message = message2 }
            };

            return Json(data);

            //return Json(series);
        }

        public async Task<Data> GetHistData(string matid, string date)
        {
            DateTime dateTime = date.ToDateTime("MM/dd/yyyy");
            string start = dateTime.ToDateTimeString();
            string end = dateTime.AddDays(1).ToDateTimeString();

            return await _respository.GetHistoryList(matid, start, end);
        }
    }
}