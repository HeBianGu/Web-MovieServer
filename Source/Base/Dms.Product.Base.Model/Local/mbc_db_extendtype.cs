using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{

    [Table("mbc_db_extendtype")]
    public class mbc_db_extendtype : StringEntityBase
    {
        [Display(Name = "扩展名称")]
        public string Name { get; set; } 

        [Display(Name = "扩展值")]
        public string Value { get; set; }

        [Display(Name = "所属类型")]
        public string MediaType { get; set; }
        

        [Display(Name = "显示名称")]
        public string Text { get; set; } 

    }
}
