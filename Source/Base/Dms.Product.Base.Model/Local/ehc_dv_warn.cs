using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dms.Product.Base.Model
{
    [Table("ehc_dv_warn")]
    public class ehc_dv_warn : StringEntityBase
    {
        [Required]
        [StringLength(10)]
        [Display(Name = "监控ID")]
        public string MONITORID { get; set; }

        [Required]
        [Display(Name = "报警类型")]
        public string WARNTYPE { get; set; }

        [Display(Name = "监护医生")]
        public string DOCTOR { get; set; }

        [Required]
        [Display(Name = "报警内容")]
        public string MESSAGE { get; set; }

        [Required]
        [Display(Name = "报警时间")]
        public string STARTTIME { get; set; } 

        [Display(Name = "持续时间")]
        public int DURATION { get; set; }
    }
}
