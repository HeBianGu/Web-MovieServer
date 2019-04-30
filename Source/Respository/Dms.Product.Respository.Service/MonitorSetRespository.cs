using Dms.Prodoct.Domain.DataServce;
using Dms.Product.Base.Model;
using Dms.Product.General.LocalDataBase;
using Dms.Product.Respository.IService;
using Dms.Product.Respository.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.Respository.Service
{

    /// <summary> 监控配置模型 </summary>
    public class MonitorSetRespository : UserLoggerRepositoryBase<ehc_dv_monitor>, IMonitorSetRespository
    {
        public MonitorSetRespository(DataContext dbcontext, ILogger<MonitorSetRespository> logger) : base(dbcontext, logger)
        {

        }

        Random r = new Random();

        public async Task<List<MonitorViewModel>> GetMonitorListTest()
        {

            var collection = await this.GetMonitorList();

            List<MonitorViewModel> result = new List<MonitorViewModel>();

            foreach (var item in collection)
            {
                int v = r.Next(50, 120);

                MonitorViewModel model = item;

                model.Heart.Description = "心率：" + v.ToString() + "次/分 历史:135/37";
                model.Breath.Description = "呼吸：" + r.Next(80, 200).ToString() + "次/分 历史:135/37";
                model.FanShen.Description = "体动：" + r.Next(6).ToString() + "分 累计：" + r.Next(12).ToString() + "时/天";
                model.Shuimian.Description = r.Next(3) == 1 ? "睡眠中/睡眠：" + r.Next(8).ToString() + "时" + r.Next(60).ToString() + "分" : "未睡眠";
                model.ZaiChuang.Description = r.Next(3) == 1 ? "离床：" + r.Next(8).ToString() + "时" + r.Next(60).ToString() + "分" : "在床：" + r.Next(8).ToString() + "时" + r.Next(60).ToString() + "分";
                model.Huli.Description = r.Next(3) == 1 ? "中度护理 计划翻身" : "中度护理 间隔翻身";

                if (v > 60 && v < 110)
                {
                    model.ForeColor = AlarmColorConfiger.ForeColor;
                    model.BackColor = AlarmColorConfiger.LeaveBedColor;
                    model.StateClass = StateClassConfiger.StateNormal;

                    model.Heart.ForeColor = AlarmColorConfiger.ForeColor;
                    model.Heart.BackColor = AlarmColorConfiger.AlarmColor;
                    model.Heart.StateClass = StateClassConfiger.StateAlarm;


                    model.Breath.ForeColor = AlarmColorConfiger.ForeColor;
                    model.Breath.BackColor = AlarmColorConfiger.AlarmColor;
                    model.Breath.StateClass = StateClassConfiger.StateNormal;
                }
                else
                {
                    model.ForeColor = AlarmColorConfiger.ForeColor;
                    model.BackColor = AlarmColorConfiger.AlarmColor;
                    model.StateClass = StateClassConfiger.StateAlarm;
                    model.Flag = 1;


                    model.Heart.ForeColor = AlarmColorConfiger.ForeColor;
                    model.Heart.BackColor = AlarmColorConfiger.AlarmColor;
                    model.Heart.StateClass = StateClassConfiger.StateAlarm;

                }


                result.Add(model);
            }

            result = result.OrderByDescending(l => l.Flag).ToList();

            return result;
        }


        static int start = 5;
        public async Task<IQueryable<jw_add_data>> GetRealLineTest()
        {
            var result = _dbContext.Datas.FromSql("select *  from jw_add_data where REGIONCODE='510703101'");

            //var result = _dbContext.Datas.FromSql("select * from jw_add_data where REGIONCODE='510703101'");

            //var find = result.Skip(r.Next(result.Count())).Take(10000);

            if (start > result.Count()) start = 5;

            var find = result.Take(start += 1).Skip(start - 20 > 0 ? start - 20 : 0).Take(10000);


            return find;
        }


        List<Tuple<string, string, string>> result = new List<Tuple<string, string, string>>();
        public async Task<List<Tuple<string, string, string>>> GetRealLine(string monitorID)
        {

            //  Do：查找匹配数据
            var realdata = this._dbContext.ehc_dv_realcaches.Where(l => l.MONITORID == monitorID && (l.DataType == DataTypeConfiger.Breath || l.DataType == DataTypeConfiger.Heart));

            Func<ehc_dv_history, string> convert = l =>
            {
                return l?.Value;
            };

            //Do：按创建时间分组,排序并获取前20条记录
            var group = realdata.GroupBy(l => l.UDATE.ToDateTime()).OrderByDescending(l => l.Key);

            DateTime nowTime = DateTime.Now;

            foreach (var item in group)
            {
                result.Add(Tuple.Create(item.Key.ToDateTimeString(),
                    realdata.FirstOrDefault(l => l.DataType == DataTypeConfiger.Heart && l.UDATE.ToDateTime() >= item.Key)?.Value,
                    realdata.FirstOrDefault(l => l.DataType == DataTypeConfiger.Breath && l.UDATE.ToDateTime() >= item.Key)?.Value));
            }

            return result;

        }

        public async Task<string> GetRealMessage(string monitorID)
        {
            var monitor = await this.GetMonitorByID(monitorID);

            var reals = await this._dbContext.ehc_dv_realdatas.Where(l => l.MONITORID == monitorID).ToListAsync();

            //  Do：心率
            var heart = reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.Heart);
            var heartmessage = $"心率：<strong>{heart?.Value}</strong>次/分 历史:<strong>{reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.HeartMax)?.Value}</strong>/<strong>{reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.HeartMin)?.Value}</strong>";

            //  Do：呼吸
            var breath = reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.Breath);
            var breathmessage = $"呼吸：<strong>{reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.Breath)?.Value}</strong>次/分 历史:<strong>{reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.BreathMax)?.Value}</strong>/<strong>{reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.BreathMin)?.Value}</strong>";

            //  Do：体动
            var moveTime = reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.MoveTime);
            var moveMessage = $"体动：<strong>{moveTime?.Value}</strong>分 累计：<strong>{reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.MoveTotal)?.Value}</strong>时/天";

            //  Do：睡眠
            var sleepTime = reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.SleepTime);
            var sleep = sleepTime == null ? TimeSpan.FromHours(0) : string.IsNullOrEmpty(sleepTime.Value) ? TimeSpan.FromHours(0) : (DateTime.Now - sleepTime.Value.ToDateTime());

            string sleepmessage = (sleepTime != null && string.IsNullOrEmpty(sleepTime.Value)) ? $"睡眠中/睡眠：<strong>{sleep.Hours}</strong>时<strong>{sleep.Minutes}</strong>分" : "未睡眠";

            //  Do：离床/在床
            var leaveTime = reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.LeaveTime);
            var leave = leaveTime == null ? TimeSpan.FromHours(0) : string.IsNullOrEmpty(leaveTime.Value) ? TimeSpan.FromHours(0) : (DateTime.Now - leaveTime.Value.ToDateTime());

            bool isleave = !string.IsNullOrEmpty(leaveTime?.Value);

            var onTime = reals.FirstOrDefault(l => l.DataType == DataTypeConfiger.OnTime);
            var on = onTime == null ? TimeSpan.FromHours(0) : string.IsNullOrEmpty(onTime.Value) ? TimeSpan.FromHours(0) : (DateTime.Now - onTime.Value.ToDateTime());
            string leavemessage = isleave ? $"离床：<strong>{leave.Hours}</strong>时<strong>{leave.Minutes}</strong>分" : $"在床：<strong>{on.Hours}</strong>时<strong>{on.Minutes}</strong>分";

            return $"<h4><strong>详细信息：</strong>{heartmessage} {heartmessage} {moveMessage} {sleepmessage} {leavemessage}</h4> ";


        }

        public async Task<List<MonitorViewModel>> GetMonitorList()
        {
            var collection = await this.GetListAsync();

            var realData = await DataService.Instance.GetRealDataCollection();

            List<MonitorViewModel> result = new List<MonitorViewModel>();

            Action<MonitorViewModel, MonitorValueItem> warnAction = (l, k) =>
            {
                //  Message：设置整体面板
                l.ForeColor = AlarmColorConfiger.ForeColor;
                l.BackColor = AlarmColorConfiger.AlarmColor;
                l.Flag = 1;
                l.StateClass = StateClassConfiger.StateAlarm;

                //  Message：设置具体项
                k.ForeColor = AlarmColorConfiger.ForeColor;
                k.BackColor = AlarmColorConfiger.AlarmColor;
                k.StateClass = StateClassConfiger.StateAlarm;
            };

            foreach (var item in collection)
            {
                MonitorViewModel model = this.GetModel(item);

                //model.ForeColor = "#FFFFFF";
                //model.BackColor = "#5E5EBD"; 

                var real = realData.data.FirstOrDefault(l => l.sn == model.Bed.MATID);

                if (real == null)
                {
                    //  Do：离线 找到对应设备
                    model.ForeColor = AlarmColorConfiger.ForeColor;
                    model.BackColor = AlarmColorConfiger.UnLineColor;
                    model.Flag = -1;
                }
                else
                {
                    //  Do：心率小于标准范围
                    if (real.heartBeat < model.MonitorDetial.HeartRange.MinValue)
                    {
                        warnAction(model, model.Heart);
                    }
                    //  Do：心率大于标准范围
                    if (real.heartBeat > model.MonitorDetial.HeartRange.MaxVlalue)
                    {
                        warnAction(model, model.Heart);
                    }

                    //  Do：呼吸小于标准范围
                    if (real.breath < model.MonitorDetial.BreathRange.MinValue)
                    {
                        warnAction(model, model.Breath);
                    }
                    //  Do：呼吸大于标准范围
                    if (real.breath > model.MonitorDetial.BreathRange.MaxVlalue)
                    {
                        warnAction(model, model.Breath);
                    }

                    //  Do：尿湿
                    if (real.wet)
                    {
                        warnAction(model, model.NiaoSi);
                    }
                }

                //model.Heart.Description = "心率：" + real?.heartBeat + "次/分 历史:无/无";
                //model.Breath.Description = "呼吸：" + real?.breath + "次/分 历史:无/无";
                //model.FanShen.Description = "体征：" + real?.state == "mov" ? "体动" : real?.state == "on" ? "在床" : real?.state == "off" ? "离床" : "呼叫";
                //model.Shuimian.Description = "尿湿：" + (real?.wet == true ? "是" : "否");
                //model.ZaiChuang.Description = "呼吸暂停次数：" + real?.odor + " 次";
                //model.Huli.Description = "身体位置：" + real?.position;

                model.Heart.Description = "心率：" + real?.heartBeat + "次/分 历史:无/无";
                model.Breath.Description = "呼吸：" + real?.breath + "次/分 历史:无/无";
                model.TiDong.Description = "体动：" + r.Next(6).ToString() + "分 累计：" + r.Next(12).ToString() + "时/天";
                model.Shuimian.Description = r.Next(3) == 1 ? "睡眠中/睡眠：" + r.Next(8).ToString() + "时" + r.Next(60).ToString() + "分" : "未睡眠";
                model.ZaiChuang.Description = r.Next(3) == 1 ? "离床：" + r.Next(8).ToString() + "时" + r.Next(60).ToString() + "分" : "在床：" + r.Next(8).ToString() + "时" + r.Next(60).ToString() + "分";
                model.Huli.Description = model.Customer.NURSE;
                model.FanShen.Description = model.Customer.TURN;

                result.Add(model);
            }

            result = result.OrderByDescending(l => l.Flag).ToList();

            return result;
        }

        public async Task<List<MonitorViewModel>> GetLocalRealList()
        {
            var reals = await this._dbContext.ehc_dv_realdatas.ToListAsync();

            var monitors = await this.GetListAsync();

            List<MonitorViewModel> result = new List<MonitorViewModel>();

            Action<MonitorViewModel, MonitorValueItem> warnAction = (l, k) =>
            {
                //  Message：设置整体面板
                l.ForeColor = AlarmColorConfiger.ForeColor;
                l.BackColor = AlarmColorConfiger.AlarmColor;
                l.Flag = 1;
                l.StateClass = StateClassConfiger.StateAlarm;

                //  Message：设置具体项
                k.ForeColor = AlarmColorConfiger.ForeColor;
                k.BackColor = AlarmColorConfiger.AlarmColor;
                k.StateClass = StateClassConfiger.StateAlarm;
            };

            var collection = reals.GroupBy(l => l.MONITORID);

            foreach (var item in collection)
            {
                var monitor = _dbContext.Moniters.FirstOrDefault(l => l.ID == item.Key);

                MonitorViewModel model = this.GetModel(monitor);

                //  Do：设备连接
                var connect = item.FirstOrDefault(l => l.DataType == DataTypeConfiger.Connect);
                model.Connect.Description = connect?.Value != "On" ? "设备离线" : "设备在线";
                model.Connect.StateClass = StateClassConfiger.Create(connect?.Value != "On" ? "1" : "0");

                //  Do：心率
                var heart = item.FirstOrDefault(l => l.DataType == DataTypeConfiger.Heart);
                model.Heart.Description = $"心率：{heart?.Value}次/分 历史:{item.FirstOrDefault(l => l.DataType == DataTypeConfiger.HeartMax)?.Value}/{item.FirstOrDefault(l => l.DataType == DataTypeConfiger.HeartMin)?.Value}";
                model.Heart.StateClass = StateClassConfiger.Create(heart?.Alarm);

                //  Do：呼吸
                var breath = item.FirstOrDefault(l => l.DataType == DataTypeConfiger.Breath);
                model.Breath.Description = $"呼吸：{item.FirstOrDefault(l => l.DataType == DataTypeConfiger.Breath)?.Value}次/分 历史:{item.FirstOrDefault(l => l.DataType == DataTypeConfiger.BreathMax)?.Value}/{item.FirstOrDefault(l => l.DataType == DataTypeConfiger.BreathMin)?.Value}";
                model.Breath.StateClass = StateClassConfiger.Create(breath?.Alarm);

                //  Do：体动
                var moveTime = item.FirstOrDefault(l => l.DataType == DataTypeConfiger.MoveTime);
                model.TiDong.Description = $"体动：{moveTime?.Value}分 累计：{item.FirstOrDefault(l => l.DataType == DataTypeConfiger.MoveTotal)?.Value}时/天";
                model.TiDong.StateClass = StateClassConfiger.Create(moveTime?.Alarm);

                //  Do：睡眠
                var sleepTime = item.FirstOrDefault(l => l.DataType == DataTypeConfiger.SleepTime);
                var sleep = sleepTime == null ? TimeSpan.FromHours(0) : string.IsNullOrEmpty(sleepTime.Value) ? TimeSpan.FromHours(0) : (DateTime.Now - sleepTime.Value.ToDateTime());

                model.Shuimian.Description = (sleepTime != null && string.IsNullOrEmpty(sleepTime.Value)) ? $"睡眠中/睡眠：{sleep.Hours}时{sleep.Minutes}分" : "未睡眠";

                //  Do：离床/在床
                var leaveTime = item.FirstOrDefault(l => l.DataType == DataTypeConfiger.LeaveTime);
                var leave = leaveTime == null ? TimeSpan.FromHours(0) : string.IsNullOrEmpty(leaveTime.Value) ? TimeSpan.FromHours(0) : (DateTime.Now - leaveTime.Value.ToDateTime());

                bool isleave = !string.IsNullOrEmpty(leaveTime?.Value);

                var onTime = item.FirstOrDefault(l => l.DataType == DataTypeConfiger.OnTime);
                var on = onTime == null ? TimeSpan.FromHours(0) : string.IsNullOrEmpty(onTime.Value) ? TimeSpan.FromHours(0) : (DateTime.Now - onTime.Value.ToDateTime());
                model.ZaiChuang.Description = isleave ? $"离床：{leave.Hours}时{leave.Minutes}分" : $"在床：{on.Hours}时{on.Minutes}分";
                model.ZaiChuang.StateClass = StateClassConfiger.Create(leaveTime?.Alarm);

                //  Do：护理
                model.Huli.Description = model.Customer.NURSE;

                var turntime = item.FirstOrDefault(l => l.DataType == DataTypeConfiger.TurnTime);
                //  Do：翻身
                model.FanShen.Description = model.Customer?.TURN == "按照间隔时间翻身护理" ? "间隔翻身" : (model.Customer?.TURN == "按照固定计划翻身护理" ? "计划翻身" : "无翻身计划");
                model.FanShen.StateClass = StateClassConfiger.Create(turntime?.Alarm);

                //  Do：尿湿
                var wet = item.FirstOrDefault(l => l.DataType == DataTypeConfiger.Wet);
                model.NiaoSi.StateClass = StateClassConfiger.Create(wet?.Alarm);

                //  Do：在床状态显示背景色
                if (!isleave)
                {
                    model.BackColor = AlarmColorConfiger.OnBedColor;
                }


                //  Do：存在报警设置整个面板报警状态
                if (item.ToList().Exists(l => l.Alarm == "1"))
                {
                    //  Message：设置整体面板
                    model.ForeColor = AlarmColorConfiger.ForeColor;
                    model.BackColor = AlarmColorConfiger.AlarmColor;
                    model.Flag = 1;
                    model.StateClass = StateClassConfiger.StateAlarm;
                }


                //  Do：存在报警设置整个面板报警状态
                if (item.ToList().TrueForAll(l => l.Alarm == "-1"))
                {
                    //  Message：设置整体面板
                    model.ForeColor = AlarmColorConfiger.ForeColor;
                    model.BackColor = AlarmColorConfiger.UnLineColor;
                    model.Flag = -1;
                    model.StateClass = StateClassConfiger.StateAlarm;
                }


                result.Add(model);
            }

            result = result.OrderByDescending(l => l.Flag).ToList();

            return result;
        }

        MonitorViewModel GetModel(ehc_dv_monitor monitor)
        {
            MonitorViewModel model = new MonitorViewModel();

            model.ID = monitor.ID.ToString();

            //  Do：用户
            var r = _dbContext.Customers.Where(l => l.ID == monitor.CUSTOMID);

            if (r != null && r.Count() > 0)
            {
                model.Customer = r.First();
            }

            //  Do：床位
            var b = _dbContext.Beds.Where(l => l.ID == monitor.BEDID);

            if (b != null && b.Count() > 0)
            {
                model.Bed = b.First();

                ////  Do：床垫
                //var m = _dbContext.Mats.Where(l => l.CODE == model.Bed.MATID);

                //if (m != null && m.Count() > 0)
                //{
                //    model.Mat = m.First();
                //}
            }

            model.MonitorDetial = this.GetMonitorDeital(monitor);

            return model;
        }

        MonitorItemViewModel GetMonitorDeital(ehc_dv_monitor monitor)
        {
            var extentions = _dbContext.ehc_dv_monitorextentions.Where(l => l.MonitorID == monitor.ID);

            MonitorItemViewModel viewModel = new MonitorItemViewModel();

            viewModel.Monitor = monitor;


            viewModel.CardID = this._dbContext.Customers.FirstOrDefault(l => l.ID == monitor.CUSTOMID)?.CARDID;

            var bed = this._dbContext.Beds.FirstOrDefault(l => l.ID == monitor.BEDID);

            viewModel.MatCode = bed?.MATID;

            var heartDb = extentions.Where(l => l.TypeID == "5d107bfa-3784-4e7c-8d40-5c9a38309cd6");

            viewModel.HeartRange = ToolService.Instance.DeSerializeObject<Splite2Template>(heartDb.FirstOrDefault().Value);

            var breathDb = extentions.Where(l => l.TypeID == "e126274b-1bc4-4724-8c84-6123a506615d");

            viewModel.BreathRange = ToolService.Instance.DeSerializeObject<Splite2Template>(breathDb.FirstOrDefault().Value);

            var timespan1Db = extentions.Where(l => l.TypeID == "5e4d6b02-1c05-4630-bfda-9c71fc031408");

            viewModel.TimeRange1 = ToolService.Instance.DeSerializeObject<TimeTemplate>(timespan1Db.FirstOrDefault().Value);

            var timespan2Db = extentions.Where(l => l.TypeID == "ee2579bd-111b-457c-8fd1-befb12bbe64e");

            viewModel.TimeRange2 = ToolService.Instance.DeSerializeObject<TimeTemplate>(timespan2Db.FirstOrDefault().Value);

            var timespan3Db = extentions.Where(l => l.TypeID == "c1a239fe-5326-4bd0-8bb3-38b9e59e36e5");

            viewModel.TimeRange3 = ToolService.Instance.DeSerializeObject<TimeTemplate>(timespan3Db.FirstOrDefault().Value);

            var timespan4Db = extentions.Where(l => l.TypeID == "1b77e4ae-81b9-45c0-bcfe-867746582dbf");

            viewModel.TimeRange4 = ToolService.Instance.DeSerializeObject<TimeTemplate>(timespan4Db.FirstOrDefault().Value);

            var timespan5Db = extentions.Where(l => l.TypeID == "67216ec5-c33c-4965-b201-c7889e46d9ef");

            viewModel.TimeRange5 = ToolService.Instance.DeSerializeObject<TimeTemplate>(timespan5Db.FirstOrDefault().Value);


            return viewModel;
        }

        public async Task<MonitorItemViewModel> GetMonitorDeitalByID(string id)
        {
            var model = await _dbContext.Moniters.FindAsync(id);

            return this.GetMonitorDeital(model);
        }

        public async Task<int> Create(MonitorItemViewModel viewModel)
        {
            ehc_dv_monitor monitor = viewModel.Monitor;

            monitor.CDATE = DateTime.Now.ToDateTimeString();
            monitor.UDATE = DateTime.Now.ToDateTimeString();

            //  Message：保存监控
            _dbContext.Add(monitor);

            //  Message：保存监控配置
            Action<TemplateBase, string> ConvertTo = (l, k) =>
            {
                ehc_dv_monitorextention extend = new ehc_dv_monitorextention();
                extend.MonitorID = monitor.ID;
                extend.TypeID = k;
                extend.Value = ToolService.Instance.SerializeObject(l);
                extend.UDATE = DateTime.Now.ToDateTimeString();
                extend.CDATE = DateTime.Now.ToDateTimeString();
                _dbContext.Add(extend);
            };

            //  Do：转换并保存
            ConvertTo(viewModel.HeartRange, "5d107bfa-3784-4e7c-8d40-5c9a38309cd6");
            ConvertTo(viewModel.BreathRange, "e126274b-1bc4-4724-8c84-6123a506615d");
            ConvertTo(viewModel.TimeRange1, "5e4d6b02-1c05-4630-bfda-9c71fc031408");
            ConvertTo(viewModel.TimeRange2, "ee2579bd-111b-457c-8fd1-befb12bbe64e");
            ConvertTo(viewModel.TimeRange3, "c1a239fe-5326-4bd0-8bb3-38b9e59e36e5");
            ConvertTo(viewModel.TimeRange4, "1b77e4ae-81b9-45c0-bcfe-867746582dbf");
            ConvertTo(viewModel.TimeRange5, "67216ec5-c33c-4965-b201-c7889e46d9ef");

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Edit(MonitorItemViewModel viewModel)
        {

            _dbContext.Update(viewModel.Monitor);

            //  Message：保存监控配置
            Action<TemplateBase, string> ConvertTo = (l, k) =>
            {
                ehc_dv_monitorextention extend = _dbContext.ehc_dv_monitorextentions.Where(m => m.MonitorID == viewModel.Monitor.ID && m.TypeID == k).FirstOrDefault();

                extend.Value = ToolService.Instance.SerializeObject(l);
                extend.UDATE = DateTime.Now.ToDateTimeString();
                _dbContext.Update(extend);
            };

            //  Do：转换并保存
            ConvertTo(viewModel.HeartRange, "5d107bfa-3784-4e7c-8d40-5c9a38309cd6");
            ConvertTo(viewModel.BreathRange, "e126274b-1bc4-4724-8c84-6123a506615d");
            ConvertTo(viewModel.TimeRange1, "5e4d6b02-1c05-4630-bfda-9c71fc031408");
            ConvertTo(viewModel.TimeRange2, "ee2579bd-111b-457c-8fd1-befb12bbe64e");
            ConvertTo(viewModel.TimeRange3, "c1a239fe-5326-4bd0-8bb3-38b9e59e36e5");
            ConvertTo(viewModel.TimeRange4, "1b77e4ae-81b9-45c0-bcfe-867746582dbf");
            ConvertTo(viewModel.TimeRange5, "67216ec5-c33c-4965-b201-c7889e46d9ef");

            return await _dbContext.SaveChangesAsync();

        }

        public async Task<bool> Exist(string id)
        {
            return await _dbContext.Moniters.AnyAsync(e => e.ID == id);

        }

        public async Task<int> Delete(string id)
        {
            //  Do：删除监控
            var models = await _dbContext.Moniters.FindAsync(id); 
            _dbContext.Moniters.Remove(models);

            //  Do：删除扩展项
            var collection = _dbContext.ehc_dv_monitorextentions.Where(l => l.MonitorID == id); 

            _dbContext.ehc_dv_monitorextentions.RemoveRange(collection);

            //  Do：删除实时数据
            var datas = _dbContext.ehc_dv_realdatas.Where(l => l.MONITORID == id); 
            _dbContext.ehc_dv_realdatas.RemoveRange(datas);

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<dynamic> GetSelectList(dynamic dynamic)
        {
            dynamic.CustomerList = new SelectList(_dbContext.Customers, "ID", "NAME");
            //dynamic.MatList = new SelectList(_dbContext.Mats, "ID", "NAME");
            dynamic.BedList = new SelectList(_dbContext.Beds, "ID", "NAME");

            return dynamic;
        }

        public async Task<IEnumerable<ehc_dv_customer>> GetCurstoms()
        {
            return await _dbContext.Customers.Where(l=>l.ISENBLED==1).ToListAsync();
        }

        //public async Task<IEnumerable<JCSJ_MAT>> GetMats()
        //{
        //    return await _dbContext.Mats.ToListAsync();
        //}

        public async Task<IEnumerable<ehc_dv_bed>> GetBeds()
        {
            return await _dbContext.Beds.Where(l => l.ISENBLED == 1).ToListAsync();
        }

        /// <summary> 获取报警记录列表 </summary>
        public async Task<IEnumerable<AlarmViewModel>> GetAlarms(string monitorID, DateTime date, string typeid)
        {
            List<ehc_dv_alarm> alarms;
            //  Do：全部
            alarms = await _dbContext.ehc_dv_alarms.Where(
                l => l.ISENBLED == 1 &&
                (string.IsNullOrEmpty(monitorID) ? true : l.MONITORID == monitorID) &&
                (string.IsNullOrEmpty(typeid) ? true : l.ALARMTYPE == typeid) &&
                (l.CDATE.ToDateTime().Date == date.Date || l.UDATE.ToDateTime().Date == date.Date)).OrderByDescending(l => l.UDATE).ToListAsync();


            List<AlarmViewModel> result = new List<AlarmViewModel>();

            var monitor = await _dbContext.Moniters.ToListAsync();

            var beds = await this.GetBeds();

            var customers = await this.GetCurstoms();

            var types = await _dbContext.ehc_dv_alarmtypes.ToListAsync();

            foreach (var item in alarms)
            {
                AlarmViewModel vm = new AlarmViewModel();

                item.DURATION = ((string.IsNullOrEmpty(item.UDATE) ? DateTime.Now : item.UDATE.ToDateTime()) - item.CDATE.ToDateTime()).ToString();

                vm.Alarm = item;


                vm.AlarmType = types?.FirstOrDefault(l => l.ID == item.ALARMTYPE);

                if (monitor != null)
                {
                    var cmonitor = monitor.FirstOrDefault(k => k.ID == item.MONITORID);

                    vm.Bed = beds?.FirstOrDefault(l => cmonitor?.BEDID == l.ID);

                    vm.Customer = customers?.FirstOrDefault(l => cmonitor?.CUSTOMID == l.ID);
                }

                result.Add(vm);
            }

            return result;
        }


        public async Task<IEnumerable<ehc_dv_alarmtype>> GetAlarmTypes()
        {
            return await _dbContext.ehc_dv_alarmtypes.ToListAsync();
        }
        /// <summary> 获取报警记录列表 </summary>
        public async Task DeleteAlarm(string id)
        {
            var model = await _dbContext.ehc_dv_alarms.FindAsync(id);

            model.ISENBLED = 0;

            _dbContext.ehc_dv_alarms.Update(model);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<MonitorViewModel> GetMonitorByID(string id)
        {
            var model = await this.GetByIDAsync(id);

            MonitorViewModel result = this.GetModel(model);

            return result;
        }

        public async Task<Data> GetHistoryList(string sn, string start, string end)
        {
            if(string.IsNullOrEmpty(sn))
            {
                return new Data();
            }

            var result = await DataService.Instance.GetHistDataCollection(sn, start, end);

            if (!result.succeed)
            {
                _logger.LogInformation("GetHistDataCollection遇到错误！");
                _logger.LogInformation(result.message);
                return null;
            }

            return result.data;

        }
        [Obsolete]
        public async Task<List<HistoryDataViewModel>> GetLocalHistoryList(string monitorID, string start, string end)
        {
            //  Do：查找匹配数据
            var realdata = this._dbContext.ehc_dv_historys.Where(l => l.MonitorID == monitorID && l.CDate.ToDateTime() > start.ToDateTime() && l.CDate.ToDateTime() < end.ToDateTime());

            Func<ehc_dv_history, string> convert = l =>
            {
                return l?.Value;
            };

            //  Do：按创建时间分组
            var group = realdata.GroupBy(l => l.CDate).Take(30);

            List<HistoryDataViewModel> result = new List<HistoryDataViewModel>();

            foreach (var item in group)
            {
                HistoryDataViewModel history = new HistoryDataViewModel();

                history.Heart = realdata.FirstOrDefault(l => l.DataType == DataTypeConfiger.Heart && l.CDate.ToDateTime() <= item.Key.ToDateTime() && l.UDate.ToDateTime() >= item.Key.ToDateTime())?.Value;
                history.Breath = realdata.FirstOrDefault(l => l.DataType == DataTypeConfiger.Breath && l.CDate.ToDateTime() <= item.Key.ToDateTime() && l.UDate.ToDateTime() >= item.Key.ToDateTime())?.Value;
                history.Move = realdata.FirstOrDefault(l => l.DataType == DataTypeConfiger.State && l.CDate.ToDateTime() <= item.Key.ToDateTime() && l.UDate.ToDateTime() >= item.Key.ToDateTime())?.Value == "mov" ? "1" : "0"; ;
                history.Leave = realdata.FirstOrDefault(l => l.DataType == DataTypeConfiger.State && l.CDate.ToDateTime() <= item.Key.ToDateTime() && l.UDate.ToDateTime() >= item.Key.ToDateTime())?.Value == "off" ? "1" : "0"; ;
                history.Sleep = realdata.FirstOrDefault(l => l.DataType == DataTypeConfiger.SleepTime && l.CDate.ToDateTime() <= item.Key.ToDateTime() && l.UDate.ToDateTime() >= item.Key.ToDateTime())?.Value == null ? "0" : "1";
                history.Date = item.Key;
                result.Add(history);
            }

            //Func<IGrouping<string, ehc_dv_history>, HistoryDataViewModel> convertTo = l =>
            //   {
            //       HistoryDataViewModel history = new HistoryDataViewModel();
            //       history.Heart = l.FirstOrDefault(k => k.DataType == DataTypeConfiger.Heart)?.Value;
            //       history.Breath = l.FirstOrDefault(k => k.DataType == DataTypeConfiger.Breath)?.Value;
            //       history.Move = l.FirstOrDefault(k => k.DataType == DataTypeConfiger.State)?.Value == "mov" ? "1" : "0";
            //       history.Leave = l.FirstOrDefault(k => k.DataType == DataTypeConfiger.State)?.Value == "off" ? "1" : "0";
            //       history.Sleep = string.IsNullOrEmpty(l.FirstOrDefault(k => k.DataType == DataTypeConfiger.SleepTime)?.Value) ? "0" : "1";
            //       history.Date = l.Key;
            //       return history;
            //   };

            //var result = await realdata.Select(l => convertTo(l)).ToListAsync();

            return null;

        }

        public async Task SaveUser(Predicate<ehc_dv_user> match, Action<ehc_dv_user> action)
        {
            var find = await this._dbContext.Users.FirstOrDefaultAsync(l => match(l));

            action(find);

            await this._dbContext.SaveChangesAsync();
        }

        public async Task<UserConfigViewModel> GetUserConfiger(Predicate<ehc_dv_user> match)
        {
            var find = await this._dbContext.Users.FirstOrDefaultAsync(l => match(l));

            UserConfigViewModel viewModel = new UserConfigViewModel();

            viewModel.UseVoice = find.USEVOICE == 1;

            return viewModel;
        }


        public async Task RefreshTurnCustom(string monitorID)
        {
            var find = this._dbContext.ehc_dv_realdatas.Where(l => l.MONITORID == monitorID);

            var result = await find.FirstOrDefaultAsync(l => l.DataType == DataTypeConfiger.TurnTime);

            result.Value = DateTime.Now.ToDateTimeString();

            result.Alarm = "0";

            this._dbContext.ehc_dv_realdatas.Update(result);

            int rr = this._dbContext.SaveChanges();


            System.Diagnostics.Debug.WriteLine(rr);

        }
    }
}
