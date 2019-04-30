using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{
    [Table("ehc_dv_bed")]
    public class ehc_dv_bed: StringEntityBase
    {

        [Required(ErrorMessage = "床位编码不能为空！")]
        [StringLength(20)]
        [Display(Name = "床位编码")]
        public string CODE { get; set; }

        [Required(ErrorMessage = "床位名称不能为空！")]
        [StringLength(20)]
        [Display(Name = "床位名称")]
        public string NAME { get; set; }

        [Required(ErrorMessage = "床垫编码不能为空！")]
        [Display(Name = "床垫编码")]
        public string MATID { get; set; }
    }
}
