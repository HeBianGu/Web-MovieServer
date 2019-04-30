using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dms.Product.Respository.Model
{
   public class HistoryDataViewModel
    {
        [Display(Name = "心率")]
        public string Heart { get; set; }

        [Display(Name = "呼吸")]
        public string Breath { get; set; }

        [Display(Name = "体动状态")]
        public string Move { get; set; }

        [Display(Name = "睡眠状态")]
        public string Sleep { get; set; }

        [Display(Name = "离床状态")]
        public string Leave { get; set; }

        [Display(Name = "创建时间")]
        public string Date { get; set; } 

        [Display(Name = "异常信息")]
        public string AlarmMessage { get; set; }
    }
}
