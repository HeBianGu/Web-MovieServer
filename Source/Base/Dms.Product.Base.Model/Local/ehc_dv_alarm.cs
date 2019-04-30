using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{
    [Table("ehc_dv_alarm")]
    public class ehc_dv_alarm : StringEntityBase
    { 
         
        [Display(Name = "监控项唯一标识")]
        public string MONITORID { get; set; }
         
        [Display(Name = "报警类型")]
        public string ALARMTYPE { get; set; }

        [Required]
        [Display(Name = "详细信息")]
        public string DESCRIPTION { get; set; } 

        [Display(Name = "持续时间")]
        public string DURATION { get; set; }

        [Display(Name = "附加信息")]
        public string TAG { get; set; }
         

    }
}
