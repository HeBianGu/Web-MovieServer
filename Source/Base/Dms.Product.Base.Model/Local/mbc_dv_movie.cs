using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{
    [Table("mbc_dv_movie")]
    public class mbc_dv_movie : StringEntityBase
    { 
         
        [Display(Name = "监控项唯一标识")]
        public string Name { get; set; }
         
        [Display(Name = "报警类型")]
        public string MediaType { get; set; }

        [Required]
        [Display(Name = "详细信息")]
        public string TagTypes { get; set; } 

        [Display(Name = "持续时间")]
        public string AreaType { get; set; }

        [Display(Name = "附加信息")]
        public string ExtendType { get; set; }

        [Display(Name = "附加信息")]
        public string ArticulationType { get; set; }

        [Display(Name = "附加信息")]
        public string Image { get; set; }

        [Display(Name = "附加信息")]
        public string VipType { get; set; }

        [Display(Name = "附加信息")]
        public string FromType { get; set; }

        [Display(Name = "附加信息")]
        public string OrderNum { get; set; }

        [Display(Name = "附加信息")]
        public string PlayCount { get; set; }

        [Display(Name = "附加信息")]
        public string Score { get; set; }


    }
}
