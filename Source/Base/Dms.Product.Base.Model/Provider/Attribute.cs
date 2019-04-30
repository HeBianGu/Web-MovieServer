using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Dms.Product.Base.Model
{
    /// <summary> 绑定监控类型表 </summary>
    public class MonitorTypeAttribute : Attribute
    {
        public static readonly DescriptionAttribute Default;

        public string MonitorTypeID { get; set; }

        public MonitorTypeAttribute(string monitorTypeID)
        {
            MonitorTypeID = monitorTypeID;
        }
    }

    /// <summary> 绑定报警类型表 </summary>
    public class AlarmTypeAttribute : Attribute
    {
        public static readonly DescriptionAttribute Default;

        public string AlarmTypeID { get; set; }

        public AlarmTypeAttribute(string alarmTypeID)
        {
            AlarmTypeID = alarmTypeID;
        }
    }

  
}
