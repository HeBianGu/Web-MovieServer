using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{
    [Table("ehc_dv_realcache")]
    public class ehc_dv_realcache : StringEntityBase
    { 
         
        [Display(Name = "监控项唯一标识")]
        public string MONITORID { get; set; }
         
        [Display(Name = "数据类型")]
        public string DataType { get; set; }

        [Required]
        [Display(Name = "实时数据值")]
        public string Value { get; set; }

        [Display(Name = "是否报警")]
        public string Alarm { get; set; } = "0";
         

    }
}
