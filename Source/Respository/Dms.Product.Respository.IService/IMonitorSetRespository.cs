
using Dms.Product.Base.Model;
using Dms.Product.Respository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.Respository.IService
{
    /// <summary> 监控配置模型接口 </summary>
    public interface IMonitorSetRespository: IUserLoggerRepositoryBase<ehc_dv_monitor>
    {
        Task<List<Tuple<string, string, string>>> GetRealLine(string monitorID);

        Task<IQueryable<jw_add_data>> GetRealLineTest();

        /// <summary> 获取监控列表 </summary>
        Task<List<MonitorViewModel>> GetMonitorList();

        /// <summary> 获取监控列表测试用 </summary>
        Task<List<MonitorViewModel>> GetMonitorListTest();

        /// <summary> 根据ID获取监控新增编辑项绑定模型 </summary>
        Task<MonitorViewModel> GetMonitorByID(string id);

        /// <summary> 根据ID获取监控新增编辑项绑定模型 </summary>
        Task<MonitorItemViewModel> GetMonitorDeitalByID(string id);

        /// <summary> 根据绑定模型创建监控项 </summary>
        Task<int> Create(MonitorItemViewModel viewModel);

        /// <summary> 根据绑定模型编辑监控项 </summary>
        Task<int> Edit(MonitorItemViewModel viewModel);

        /// <summary> 指定监控ID是否存在 </summary>
        Task<bool> Exist(string id);

        /// <summary> 删除指定监控项 </summary>
        Task<int> Delete(string id);

        /// <summary> 获取所有客户列表 </summary>
        Task<IEnumerable<ehc_dv_customer>> GetCurstoms();

        ///// <summary> 获取所有床垫列表 </summary>
        //Task<IEnumerable<ehc_dd_device>> GetMats();

        /// <summary> 获取所有床列表 </summary>
        Task<IEnumerable<ehc_dv_bed>> GetBeds();

        /// <summary> 获取监控列表 </summary>
        Task<Data> GetHistoryList(string sn,string start,string end);

        /// <summary> 获取本地监控历史列表 </summary>
        Task<List<HistoryDataViewModel>> GetLocalHistoryList(string monitorID, string start, string end);

        /// <summary> 获取报警记录列表 </summary>
        Task<IEnumerable<AlarmViewModel>> GetAlarms(string monitorID, DateTime date, string typeid);

        /// <summary> 删除报警记录 </summary>
        Task DeleteAlarm(string id);

        /// <summary> 获取报警类型列表 </summary>
        Task<IEnumerable<ehc_dv_alarmtype>> GetAlarmTypes();

        /// <summary> 获取本地实时数据 </summary>
        Task<List<MonitorViewModel>> GetLocalRealList();

        /// <summary> 保存用户信息 </summary>
        Task SaveUser(Predicate<ehc_dv_user> match, Action<ehc_dv_user> action);

        /// <summary> 获取用户配置信息 </summary>
        Task<UserConfigViewModel> GetUserConfiger(Predicate<ehc_dv_user> match);

        /// <summary> 用户翻身更新 </summary>
        Task RefreshTurnCustom(string monitorID);

        /// <summary> 获取实时信息字符串信息 </summary>
        Task<string> GetRealMessage(string monitorID);
    }
}
