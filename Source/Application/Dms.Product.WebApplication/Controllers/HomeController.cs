using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dms.Product.WebApplication.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.ComponentModel;
using Dms.Product.Respository.IService;
using Dms.Product.Respository.Model;
using Dms.Product.General.ThridTool;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Dms.Prodoct.Domain.DataServce;
using Dms.Product.Base.Model;

namespace Dms.Product.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        IUserAccountRespositroy _respository;

        public HomeController(IUserAccountRespositroy respository)
        {
            _respository = respository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            var methodname = MethodBase.GetCurrentMethod();

            Debug.WriteLine(methodname);

            ////跳转到系统首页
            //return RedirectToAction("Monitor", "Monitor");

            if (ModelState.IsValid)
            {

                var desPassWord = ToolService.Instance.EncryptByDES(model.Password);
                //检查用户信息 
                var user = await _respository.FirstOrDefaultAsync(l => l.NAME == model.UserName && l.PASSWORD == desPassWord);

                if (user != null)
                {
                    //记录Session
                    HttpContext.Session.SetString("CurrentUserId", user.ID.ToString());
                    HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));
                    //HttpContext.Session.SetString("CurrentUserRole", user.ROLEID.ToString())); 
                    //var type = MethodBase.GetCurrentMethod().GetCustomAttributes<DescriptionAttribute>();   

                   

                    //  Message：验证用户权限
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Sid, user.ID.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.USERNAME));
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.ROLEID));
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));  

                    await this._respository.WriteUserLogger(user.ID, UserLoggerConfiger.Login, user.NAME);

                    //跳转到系统首页
                    return RedirectToAction("Index", "Movie");

                }

                ViewBag.ErrorInfo = "提示:用户名或密码错误。";

           

                return View();
            }

            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                    break;
                }
            }

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //  Message：每个控制器的基类Controler 都包含两个过滤器，这个在过滤器之后调用，下面在过滤器之前调用
        public override void OnActionExecuted(ActionExecutedContext context)
        {

            Debug.WriteLine("OnActionExecuted");

            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.WriteLine("OnActionExecuting");

            base.OnActionExecuting(context);
        }
    }
}
