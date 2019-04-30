using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{

    [Table("ehc_dv_monitortype")]
    public class ehc_dv_monitortype : StringEntityBase
    {
        [Display(Name = "类型名称")]
        public string Name { get; set; } 

        [Display(Name = "报警类型ID")]
        public string AlarmTypeID { get; set; }

        [Display(Name = "数据类型")]
        public string DataType { get; set; }

        [Display(Name = "模板类型")]
        public string Template { get; set; }

        [Display(Name = "排序字段")]
        public int OrderBy { get; set; }

    }
}
