using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{
    [Table("ehc_dv_alarmtype")]
    public class ehc_dv_alarmtype : StringEntityBase
    { 
         
        [Display(Name = "报警类型")]
        public string NAME { get; set; }

    }
}
