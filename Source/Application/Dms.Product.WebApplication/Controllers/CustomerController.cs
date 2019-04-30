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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRespository _respository;

        public CustomerController(ICustomerRespository respository)
        {
            _respository = respository;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            return View(await _respository.GetListAsync());
        }

        // GET: Customer/Details/5
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

            return View(model);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ehc_dv_customer model)
        {
            if (ModelState.IsValid)
            {
                await _respository.InsertAsync(model);

                await _respository.WriteUserLogger(this.GetUserID(), "添加用户", model.NAME);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Customer/Edit/5
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
            return View(model);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ehc_dv_customer model)
        {
            if (id != model.ID.ToString())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _respository.UpdateAsync(model);

                    await _respository.WriteUserLogger(this.GetUserID(), "编辑用户", model.NAME);
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
            return View(model);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(string id)
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

            return View(model);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Enbled(string id)
        {
            var model = await _respository.GetByIDAsync(id);

            model.ISENBLED = 0;

            model.OUTDATE = DateTime.Now.ToDateTimeString();

            await _respository.UpdateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _respository.DeleteAsync(id);

            await _respository.WriteUserLogger(this.GetUserID(), UserLoggerConfiger.DeleteCustom, id);

            return RedirectToAction(nameof(Index));
        }
    }
}
