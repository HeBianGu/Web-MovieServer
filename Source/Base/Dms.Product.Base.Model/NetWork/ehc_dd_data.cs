using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dms.Product.Base.Model
{
    public class ehc_dd_data
    {
        /// <summary>
        /// 设备唯一序列号
        /// </summary>
        public string sn { get; set; }
        /// <summary>
        /// 体征状态枚举：On=在床，off-离床,mov=体动,call=呼叫
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 心跳频率
        /// </summary>
        [Display(Name ="心率")]
        public int? heartBeat { get; set; }
        /// <summary>
        /// 呼吸频率
        /// </summary>
        [Display(Name = "呼吸")]
        public int? breath { get; set; }
        /// <summary>
        /// 尿湿：true 0xc3为尿湿false0xc2 为非尿湿
        /// </summary>
        public bool wet { get; set; }
        /// <summary>
        /// 呼吸暂停次数24小时清零，3秒一次发送， 累加
        /// </summary>
        public int? odor { get; set; }
        /// <summary>
        /// 在床 离床的 辅助判断 详细见后面说明 重量:-1表示没有安装设备
        /// </summary>
        public int? weight { get; set; }
        /// <summary>
        /// 身体位置，例如[6,9]
        /// </summary>
        public string position { get; set; }
        /// <summary>
        /// 数据创建时间
        /// </summary>
        public string createTime { get; set; }
    }
}
