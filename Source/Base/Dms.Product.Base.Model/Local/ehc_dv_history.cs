using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{
    [Table("ehc_dv_history")]
    public class ehc_dv_history : EntityBase<string>
    {
        public ehc_dv_history()
        {
            this.ID = Guid.NewGuid().ToString();

            this.CDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            this.UDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        [Display(Name = "监控项唯一标识")]
        public string MonitorID { get; set; }

        [Display(Name = "数据类型")]
        public string DataType { get; set; }

        [Required]
        [Display(Name = "数据值")]
        public string Value { get; set; }

        [Display(Name = "创建时间")]
        public string CDate { get; set; }

        [Display(Name = "更新时间")]
        public string UDate { get; set; }


    }
}
