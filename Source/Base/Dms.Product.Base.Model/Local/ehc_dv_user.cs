using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dms.Product.Base.Model
{
    [Table("ehc_dv_user")]
    public class ehc_dv_user : StringEntityBase
    {
        [Required(ErrorMessage = "用户名不能为空！")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "用户名长度在3-10范围内！")]
        [Display(Name = "用户名")]
        public string NAME { get; set; }

        [Required(ErrorMessage = "用户密码不能为空！")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度在6-20范围内！")]
        [Display(Name = "登录密码")]
        public string PASSWORD { get; set; }

        [Display(Name = "状态")]
        public string STATE { get; set; }


        [Display(Name = "用户类型")]
        public string ROLEID { get; set; }

        [Display(Name = "电话号码")]
        public string TEL { get; set; }

        [Display(Name = "监控是否提示音")]
        public int USEVOICE { get; set; }

        [Required(ErrorMessage = "用户姓名不能为空！")]
        [Display(Name = "用户姓名")]
        public string USERNAME { get; set; }
    }
}
