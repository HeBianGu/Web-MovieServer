using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dms.Product.Base.Model;

namespace Dms.Product.Respository.Model
{
   public class AlarmViewModel
    {
        [Display(Name = "床位")]
        public ehc_dv_bed Bed { get; set; }

        [Display(Name = "用户")]
        public ehc_dv_customer Customer { get; set; }

        [Display(Name = "报警类型")]
        public ehc_dv_alarmtype AlarmType { get; set; }

        [Required]
        [Display(Name = "报警信息")]
        public ehc_dv_alarm Alarm { get; set; }
    }
}
