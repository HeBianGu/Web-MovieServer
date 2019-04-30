using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{
    [Table("jw_add_data")]
    public class jw_add_data : StringEntityBase
    {

        [Display(Name = "区划ID")]
        public string ORGID { get; set; }

        [Display(Name = "区划名称")]
        public string ORGNAME { get; set; }

        [Display(Name = "区划类型")]
        public string ORGTYPE { get; set; }

        [Display(Name = "区划编码")]
        public string REGIONCODE { get; set; }

        [Display(Name = "0-6岁儿童建档总数")]
        public int? CDTOTAL { get; set; }

        [Display(Name = "孕妇建档总数")]
        public int? CYTOTAL { get; set; }

        [Display(Name = "糖尿病建档总数")]
        public int? DBTOTAL { get; set; }

        [Display(Name = "建档总数")]
        public int? FILETOTAL { get; set; }


        [Display(Name = "高血压建档总数")]
        public int? HYTENTOTAL { get; set; }


        [Display(Name = "老年人建档总数")]
        public int? OLDTOTAL { get; set; }

        [Display(Name = "老年人完善建档总数")]
        public int? PERFECTOLDTOTAL { get; set; }
        [Display(Name = "完善建档总数")]
        public int? PERFECTTOTLE { get; set; }


        [Display(Name = "重精建档总数")]
        public int? SPTOTAL { get; set; }

        [Display(Name = "合计签约数量")]
        public int? COUNT { get; set; }

        [Display(Name = "孕产妇签约数")]
        public int? WOMANRATE { get; set; }

        [Display(Name = "糖尿病签约数")]
        public int? TNBRATE { get; set; }


        [Display(Name = "贫困签约率")]
        public int? PKPRATE { get; set; }
         


        [Display(Name = "老年人签约数")]
        public int? OLDMANRATE { get; set; }

        [Display(Name = "高血压签约数")]
        public int? GXYRATE { get; set; }


        [Display(Name = "普通人签约数")]
        public int? CMPRATE { get; set; }

        [Display(Name = "儿童签约数")]
        public int? CHILDRATE { get; set; }

        [Display(Name = "年")]
        public int? YEAR { get; set; }


        [Display(Name = "类型")]
        public int? TYPE { get; set; }

        [Display(Name = "日期")]
        public DateTime? UDATE { get; set; }
        
    }
}
