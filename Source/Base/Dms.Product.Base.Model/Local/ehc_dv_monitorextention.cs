using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{ 
    [Table("ehc_dv_monitorextention")]
    public class ehc_dv_monitorextention : StringEntityBase
    { 
        [Required]
        [Display(Name = "监控项ID")]
        public string MonitorID { get; set; }

        [Display(Name = "类别项ID")]
        public string TypeID { get; set; }

        [Display(Name = "检测参数值")]
        public string Value { get; set; }
    }
}
