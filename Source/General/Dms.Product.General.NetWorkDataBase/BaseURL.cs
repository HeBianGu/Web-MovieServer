using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.General.NetWorkDataBase
{
    /// <summary>
    /// 网络请求中所需要用到的URL
    /// </summary>
    public class BaseURL
    {
        const string urlFormat = " http://{0}:{1}{2}";

        //const string ip = "121.40.75.174";

        const string ip = "192.168.5.133";

        const string port = "7777";
        string GetServiceUrl(string serviceTypeString)
        {
            return string.Format(urlFormat, ip, port, serviceTypeString);
        }

        public string GetServiceUrl(URLEnum urlEnum)
        {
            return this.GetServiceUrl(this.GetURL(urlEnum));
        }

        /// <summary>
        /// 根据相应的Enum类型以及服务器环境返回相应的URL
        /// </summary>
        /// <param name="urlEnum"></param>
        /// <returns></returns>
        string GetURL(URLEnum urlEnum)
        {
            FieldInfo field = urlEnum.GetType().GetField(urlEnum.ToString());
            var desc = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return desc.Description;

        }
    }
    public enum URLEnum
    {
        /// <summary> 历史数据 </summary>
        [Description("/DaPaRe/FindPastRecordsData")]
        FindPastRecordsData = 0,
        /// <summary> 实时数据 </summary>
        [Description("/DaReTi/FindRealTimeData")]
        FindRealTimeData,
        /// <summary> 设备列表 </summary>
        [Description("/DeIn/FindDeviceList")]
        FindDeviceList,
        /// <summary> 签退接口 </summary>
        [Description("/register/updateObservByMykh.do")]
        updateObservByMykh,
        /// <summary> 儿保现场取号 </summary>
        [Description("/ebqh/ebxcqh.do")]
        ebqhByTel,
        /// <summary> 儿保主页面统计数据 </summary>
        [Description("/ebqh/getPhxx.do")]
        getPhxx,
        /// <summary> 手机号取号（使用HIS系统的手机取号） </summary>
        [Description("/ebqh/ebqhOld.do")]
        ebqhOld,
        /// <summary> 手机号取号（不使用HIS系统的手机取号） </summary>
        [Description("/ebqh/ebqhNew.do")]
        ebqhNew
    }
}
