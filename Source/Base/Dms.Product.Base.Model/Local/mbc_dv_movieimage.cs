﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{
    [Table("mbc_dv_movieimage")]
    public class mbc_dv_movieimage : StringEntityBase
    {
        [Required]
        [Display(Name = "所属电影")]
        public string MovieID { get; set; }
         
        [Display(Name = "图片内容")]
        public string Image { get; set; }
         

        [Display(Name = "显示名称")]
        public string Text { get; set; }
         
        [Display(Name = "排序")]
        public string OrderNum { get; set; }
         
        [Display(Name = "时间点")]
        public string TimeSpan { get; set; } 
 

    }
}
