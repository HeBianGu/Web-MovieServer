using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dms.Product.Base.Model
{
    [Table("ehc_dv_userlogger")]
    public class ehc_dv_userlogger : StringEntityBase
    {
        [Required]
        [StringLength(10)]
        [Display(Name = "操作用户")]
        public string USERID { get; set; }

        [Required]
        [Display(Name = "操作类型")]
        public string TYPE { get; set; } = "system";

        [Display(Name = "日志内容")]
        public string MESSAGE { get; set; }

        [Required]
        [Display(Name = "操作时间")]
        public string TIME { get; set; } 
    }
}
