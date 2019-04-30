using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Dms.Product.Base.Model;
using Dms.Product.Respository.IService;
using Dms.Prodoct.Domain.DataServce;
using Dms.Product.Respository.Model;

namespace Dms.Product.WebApplication.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserAccountRespositroy _respository;

        public UserController(IUserAccountRespositroy respository)
        {
            _respository = respository;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var result = await _respository.GetUsers();

            return View(result);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _respository.GetByIDAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            var roles = await _respository.GetRoles();

            string roleName = roles?.FirstOrDefault(l => l.ID == model.ROLEID)?.NAME;

            model.PASSWORD = ToolService.Instance.DecryptByDES(model.PASSWORD);

            var result = Tuple.Create(model, roleName);

            return View(result);
        }

        // GET: User/Create
        public async Task<IActionResult> Create()
        {
            await this.RefreshDropList();

            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,NAME,PASSWORD,USERNAME,TEL,ROLEID")] ehc_dv_user model)
        {
            if (ModelState.IsValid)
            {
                model.CDATE = DateTime.Now.ToDateTimeString();
                model.UDATE = DateTime.Now.ToDateTimeString();
                model.ISENBLED = 1;
                model.PASSWORD = ToolService.Instance.EncryptByDES(model.PASSWORD);

                await _respository.InsertAsync(model);

                await _respository.WriteUserLogger(this.GetUserID(), UserLoggerConfiger.AddUser, model.USERNAME);

                return RedirectToAction(nameof(Index));
            }
            

            await this.RefreshDropList();

            return View(model);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _respository.GetByIDAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            model.PASSWORD = ToolService.Instance.DecryptByDES(model.PASSWORD);

            await this.RefreshDropList();

            return View(model);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,NAME,PASSWORD,USERNAME,TEL,ROLEID")] ehc_dv_user model)
        {
            if (id != model.ID.ToString())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.PASSWORD = ToolService.Instance.EncryptByDES(model.PASSWORD);

                    await _respository.UpdateAsync(model);

                    await _respository.WriteUserLogger(this.GetUserID(), UserLoggerConfiger.EditUser, model.USERNAME);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var result = await _respository.GetByIDAsync(id);

                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
             

            await this.RefreshDropList();

            return View(model);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            await _respository.DeleteAsync(id);

            await _respository.WriteUserLogger(this.GetUserID(), UserLoggerConfiger.DeleteUser, id);

            return RedirectToAction(nameof(Index));
        }


        /// <summary> 刷新新建下拉列表 </summary>
        async Task RefreshDropList()
        {
            var roles = await _respository.GetRoles();

            ViewBag.RoleList = new SelectList(roles, "ID", "NAME");
        }
    }
}
