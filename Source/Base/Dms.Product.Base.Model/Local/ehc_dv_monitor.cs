using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{
    [Table("ehc_dv_monitor")]
    public class ehc_dv_monitor : StringEntityBase
    {
        [Required(ErrorMessage = "床位编码不能为空！")]
        [Display(Name = "床位编码")]
        public string BEDID { get; set; }

        [Required(ErrorMessage = "用户姓名不能为空！")]
        [Display(Name = "用户姓名")]
        public string CUSTOMID { get; set; } 
  
    }
}
