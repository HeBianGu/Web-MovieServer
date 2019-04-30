
using Dms.Product.Base.Model;
using Dms.Product.General.NetWorkDataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Prodoct.Domain.DataServce
{
   public class DataService
    {
        public static DataService Instance = new DataService();


        NetWorkService _netWorkService = new NetWorkService();

        /// <summary>
        /// 获取历史信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sn"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public async Task<HistoryRoot> GetHistDataCollection(string sn, string startTime, string endTime)
        {
            return await _netWorkService.GetHistDataCollection(sn, startTime, endTime);
        }

        public async Task<NetWorkDataCollection<ehc_dd_data>> GetRealDataCollection()
        {
            return await _netWorkService.GetDataBase<ehc_dd_data>(URLEnum.FindRealTimeData);
        }

        public async Task<NetWorkDataCollection<ehc_dd_device>> GetDevice()
        {
            return await _netWorkService.GetDataBase<ehc_dd_device>(URLEnum.FindDeviceList);
        }
    }
}
