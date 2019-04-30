using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dms.Product.Base.Model
{
    public class ehc_dd_device
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string deviceID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int deviceType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lastModifyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string valid { get; set; }
    }
}
