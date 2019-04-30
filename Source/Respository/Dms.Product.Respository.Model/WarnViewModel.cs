using Dms.Product.Base.Model; 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dms.Product.Respository.Model
{
    class WarnViewModel
    {

        [Required]
        [Display(Name = "床位编号")]
        public ehc_dv_bed Bed { get; set; }

        [Display(Name = "用户姓名")]
        public ehc_dv_user User { get; set; }

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
