
using Dms.Product.Base.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Dms.Product.Respository.Model
{

    public class UserConfigViewModel
    {

        public bool UseVoice { get; set; } = true;
        public bool DisplayType1 { get; set; } = true;
        public bool DisplayType2 { get; set; } = true;

    }
    public class MonitorViewModel
    {
        public string ID { get; set; }

        public MonitorItemViewModel MonitorDetial { get; set; } = new MonitorItemViewModel();

        public ehc_dv_customer Customer { get; set; } = new ehc_dv_customer();

        public ehc_dv_bed Bed { get; set; } = new ehc_dv_bed();

        //public JCSJ_MAT Mat { get; set; } = new JCSJ_MAT();

        public MonitorValueItem Heart { get; set; } = new MonitorValueItem();

        public MonitorValueItem Breath { get; set; } = new MonitorValueItem();

        public MonitorValueItem TiDong { get; set; } = new MonitorValueItem();

        public MonitorValueItem Shuimian { get; set; } = new MonitorValueItem();

        public MonitorValueItem ZaiChuang { get; set; } = new MonitorValueItem();

        public MonitorValueItem NiaoSi { get; set; } = new MonitorValueItem();

        public MonitorValueItem Huli { get; set; } = new MonitorValueItem();

        public MonitorValueItem FanShen { get; set; } = new MonitorValueItem();

        public MonitorValueItem Connect { get; set; } = new MonitorValueItem();

        /// <summary> 字体颜色 </summary>
        public string ForeColor { get; set; } = AlarmColorConfiger.ForeColor;

        /// <summary> 背景颜色 </summary>
        public string BackColor { get; set; } = AlarmColorConfiger.LeaveBedColor;

        /// <summary> 闪烁的颜色 </summary>
        public string FlashColor { get; set; } = AlarmColorConfiger.FlashColor;

        public int Flag { get; set; } = 0;

        public string StateClass { get; set; } = StateClassConfiger.StateNormal;

    }

    /// <summary> 具体检测项目展示的信息 </summary>
    public class MonitorValueItem
    {
        /// <summary> 显示的信息 </summary>
        public string Description { get; set; }

        /// <summary> 字体颜色 </summary>
        public string ForeColor { get; set; } = AlarmColorConfiger.ForeColor;

        /// <summary> 背景颜色 </summary>
        public string BackColor { get; set; } = AlarmColorConfiger.LeaveBedColor;

        /// <summary> 闪烁的颜色 </summary>
        public string FlashColor { get; set; } = AlarmColorConfiger.FlashColor;

        /// <summary> 展现的样式 (报警还是没有报警) </summary>
        public string StateClass { get; set; } = StateClassConfiger.StateNormal;
    }


    public class DataTypeConfiger
    {
        public const string Heart = "Heart";
        public const string HeartMax = "HeartMax";
        public const string HeartMin = "HeartMax";
        public const string Breath = "Breath";
        public const string BreathMax = "BreathMax";
        public const string BreathMin = "BreathMin";
        public const string Wet = "Wet";
        public const string MoveTime = "MoveTime";
        public const string MoveTotal = "MoveTotal";
        public const string OnTime = "OnTime";
        public const string LeaveTime = "LeaveTime";
        public const string SleepTime = "SleepTime";
        public const string TurnTime = "TurnTime";
        public const string State = "State";
        public const string Connect = "Connect";

    }

    public class AlarmColorConfiger
    {
        public const string LeaveBedColor = "#76DCFF";
        public const string OnBedColor = "#7E96EE";
        public const string AlarmColor = "#FF834E";
        public const string ForeColor = "#FFFFFF";
        public const string UnLineColor = "#F5F5F5";
        public const string FlashColor = "#252525";
    }

    public class UserLoggerConfiger
    {
        public const string AddUser = "添加用户";
        public const string EditUser = "编辑用户";
        public const string DeleteUser = "删除用户";
        public const string Login = "用户登录";
        public const string AddBed = "添加床位";
        public const string EditBed = "编辑床位";
        public const string DeleteBed = "删除床位";

        public const string AddMonitor = "添加监控";
        public const string EditMonitor = "编辑监控";
        public const string DeleteMonitor = "删除监控";

        public const string DeleteAlarm = "删除报警";

        public const string AddCustom = "添加客户";
        public const string EditCustom = "编辑客户";
        public const string DeleteCustom = "删除客户";
        public static List<string> GetAllType()
        {
            return typeof(UserLoggerConfiger).GetFields().Select(l => l.GetRawConstantValue().ToString()).ToList();
        }
    }

    public class StateClassConfiger
    {
        public const string StateAlarm = "statealarm";
        public const string StateNormal = "";

        public static string Create(string value)
        {
            return value == "1" ? StateAlarm : StateNormal;
        }
    }
}
