using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{

    [Table("mbc_dc_case")]
    public class mbc_dc_case : StringEntityBase
    {
        [Display(Name = "案例名称")]
        public string Name { get; set; } 

        [Display(Name = "案例路径")]
        public string BaseUrl { get; set; } 

    }
}
