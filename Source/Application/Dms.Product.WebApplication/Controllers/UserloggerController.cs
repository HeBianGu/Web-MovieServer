using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dms.Product.Base.Model;
using Dms.Product.Respository.IService;
using Dms.Product.Respository.Model;

namespace Dms.Product.WebApplication.Controllers
{
    public class UserloggerController : ControllerBase
    {
        private readonly IUserLoggerRespository _respository;

        public UserloggerController(IUserLoggerRespository respository)
        {
            _respository = respository;
        }

        // GET: Userlogger
        public async Task<IActionResult> Index()
        {
            await this.RefreshDropList();

            //var result = await _respository.GetAllLoggersAsync();

            return View();
        } 

        [HttpPost]
        [Route("Userlogger/GetUserloggerData")]
        public PartialViewResult GetUserloggerData(string userid, string type, string date)
        { 

            DateTime dateTime = date.ToDateTime("MM/dd/yyyy");

            Predicate<ehc_dv_userlogger> macth = l =>
            {
                return (userid == "-全部-" ? true : l.USERID == userid) && (type == "-全部-" ? true : l.TYPE == type) && l.CDATE.ToDateTime().Date == dateTime.Date;
            };

            var models = this._respository.GetLoggersBy(macth);

            return PartialView("_UserLoggerView", models.Result);

        }
         
        // GET: Userlogger/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var ehc_dv_userlogger = await _context.ehc_dv_userloggers
            //    .FirstOrDefaultAsync(m => m.ID == id);
            //if (ehc_dv_userlogger == null)
            //{
            //    return NotFound();
            //}

            return View(null);
        }

        // POST: Userlogger/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //var ehc_dv_userlogger = await _context.ehc_dv_userloggers.FindAsync(id);
            //_context.ehc_dv_userloggers.Remove(ehc_dv_userlogger);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary> 刷新新建下拉列表 </summary>
        async Task RefreshDropList()
        {
            var users = await this._respository.GetUsers();

            ViewBag.Users = new SelectList(users, "ID", "NAME");

            var types = UserLoggerConfiger.GetAllType();

            ViewBag.Types = new SelectList(types); 
        }

    }
}
