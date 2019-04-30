using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{
    [Table("ehc_dv_role")]
    public class ehc_dv_role : StringEntityBase
    {
        [Display(Name = "角色名称")]
        public string NAME { get; set; } 

        [Display(Name = "父角色ID")]
        public string PARENT { get; set; }

        [Display(Name = "描述信息")]
        public string DISCRPTION { get; set; }

    }
}
