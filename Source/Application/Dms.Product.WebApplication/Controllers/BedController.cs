using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dms.Product.Base.Model;
using Dms.Product.Respository.IService;

namespace Dms.Product.WebApplication.Controllers
{
    public class BedController : ControllerBase
    {
        private readonly IBedRespository _respository;

        public BedController(IBedRespository respository)
        {
            _respository = respository;
        }

        // GET: Bed
        public async Task<IActionResult> Index()
        {
            return View(await _respository.GetListAsync());
        }

        // GET: Bed/Details/5
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

        // GET: Bed/Create
        public async Task<IActionResult> Create()
        {
            var beds = await this._respository.GetListAsync();

            Func<ehc_dd_device, bool> match = l => !beds.Exists(k => k
           .MATID == l.deviceID);

            await this.RefreshDropList(match);


            return View();
        }

        // POST: Bed/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CODE,NAME,MATID,CDATE,UDATE")] ehc_dv_bed model)
        {
            if (ModelState.IsValid)
            {
                await _respository.InsertAsync(model);

                await this._respository.WriteUserLogger(this.GetUserID(), "添加床位", model.CODE);

                return RedirectToAction(nameof(Index));
            }


            var beds = await this._respository.GetListAsync();

            Func<ehc_dd_device, bool> match = l => !beds.Exists(k => k
             .MATID == l.deviceID);

            await this.RefreshDropList(match);

            return View(model);
        }

        // GET: Bed/Edit/5
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


            var beds = await this._respository.GetListAsync();

            Func<ehc_dd_device, bool> match = l => (!beds.Exists(k => k.MATID == l.deviceID)) || l.deviceID == model.MATID;

            await this.RefreshDropList(match);


            return View(model);
        }

        // POST: Bed/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,CODE,NAME,CDATE,UDATE")] ehc_dv_bed model)
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


                    await this._respository.WriteUserLogger(this.GetUserID(), "编辑床位", model.CODE);
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

        // GET: Bed/Delete/5
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

        // POST: Bed/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _respository.DeleteAsync(id);

            await this._respository.WriteUserLogger(this.GetUserID(), "删除床位", id);

            return RedirectToAction(nameof(Index));
        }

        /// <summary> 刷新新建下拉列表 </summary>
        async Task RefreshDropList(Func<ehc_dd_device, bool> match)
        {
            var collection = await _respository.GetMats();


            collection = collection.Where(l => match(l));

            ViewBag.MatList = new SelectList(collection, "deviceID", "deviceID");
        }

    }
}
