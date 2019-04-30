using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dms.Product.Base.Model
{
    [Table("ehc_dv_customer")]
    public class ehc_dv_customer : StringEntityBase
    {


        public ehc_dv_customer():base()
        {
            this.INDATE = DateTime.Now.ToString("yyyy-MM-dd");
            //this.OUTDATE = DateTime.Now.ToString("yyyy-MM-dd");

            //this.IMAGE = "../images/user.png";
        }
        [Required(ErrorMessage = "用户姓名不得为空")]
        [Display(Name = "用户姓名")]
        public string NAME { get; set; }

        [Display(Name = "照片")]
        public string IMAGE { get; set; }

        [Required(ErrorMessage = "性别不得为空")]
        [Display(Name = "性别")]
        public string SEX { get; set; }

        [Required(ErrorMessage = "身份证号不得为空")]
        [Display(Name = "身份证号")] 
        public string CARDID { get; set; }

        [Required(ErrorMessage = "年龄不得为空")]
        [Range(1, 120, ErrorMessage = "年龄必须介于1~120之间")]
        [Display(Name = "年龄")]
        public string AGE { get; set; }

        [Required]
        [Phone]
        [Display(Name = "电话")]
        public string TEL { get; set; }

        [Required(ErrorMessage = "紧急联系人不得为空")]
        [Display(Name = "紧急联系人")]
        public string CONTACT { get; set; }
     
        [Display(Name = "入院诊断")]
        public string DIAGNOSIS { get; set; }
      
        [Display(Name = "病史")]
        public string HISTORY { get; set; }

        [Required(ErrorMessage = "护理等级不得为空")]
        [Display(Name = "护理等级")]
        public string NURSE { get; set; }

        [Required(ErrorMessage = "翻身护理不得为空")]
        [Display(Name = "翻身护理")]
        [AlarmType("5654b386-1c7b-45d7-b09a-56228f12839b")]
        public string TURN { get; set; }

        [Display(Name = "翻身护理时间间隔")]
        public int TIMESPAN { get; set; }

        [Display(Name = "入院时间")]
        public string INDATE { get; set; }

        [Display(Name = "出院时间")]
        public string OUTDATE { get; set; }


    }
}
