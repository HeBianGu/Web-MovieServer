using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dms.Product.Respository.Model
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "提示:用户名不能为空。")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "提示:密码不能为空。")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
