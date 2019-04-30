
using Dms.Product.Base.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dms.Product.Respository.Model
{
    public class UserLoggerViewModel
    {
        [Required]
        [Display(Name = "操作用户")]
        public ehc_dv_user User { get; set; }

        [Required]
        [Display(Name = "操作类型")]
        public string TYPE { get; set; }

        [Display(Name = "日志内容")]
        public string MESSAGE { get; set; }

        [Required]
        [Display(Name = "操作时间")]
        public string TIME { get; set; }
    }
}
