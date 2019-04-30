using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{

    [Table("mbc_db_fromtype")]
    public class mbc_db_fromtype : StringEntityBase
    {
        [Display(Name = "类型名称")]
        public string Name { get; set; } 

        [Display(Name = "报警类型ID")]
        public string Value { get; set; }

        [Display(Name = "数据类型")]
        public string Text { get; set; } 

    }
}
