using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dms.Product.Base.Model;
using Dms.Product.Respository.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dms.Product.WebApplication.Controllers
{
    public class CaseController : ControllerBase
    {
        private readonly ICaseRespository _respository;

        public CaseController(ICaseRespository respository)
        {
            _respository = respository;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {
            return View(await _respository.GetListAsync());
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mbc_dv_movie = await _respository.GetByIDAsync(id);
            if (mbc_dv_movie == null)
            {
                return NotFound();
            }

            return View(mbc_dv_movie);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(mbc_dc_case mbc_dv_movie)
        {
            if (ModelState.IsValid)
            {
                await _respository.InsertAsync(mbc_dv_movie);

                return RedirectToAction(nameof(Index));
            }
            return View(mbc_dv_movie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mbc_dv_movie = await _respository.GetByIDAsync(id);
            if (mbc_dv_movie == null)
            {
                return NotFound();
            }
            return View(mbc_dv_movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,MediaType,TagTypes,AreaType,ExtendType,ArticulationType,Image,VipType,FromType,OrderNum,PlayCount,Score,CDATE,UDATE,ISENBLED,ID")] mbc_dv_movie mbc_dv_movie)
        {
            if (id != mbc_dv_movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var model = await _respository.GetByIDAsync(id);
                    await _respository.UpdateAsync(model);
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (_respository.GetByIDAsync(mbc_dv_movie.ID) == null)
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
            return View(mbc_dv_movie);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mbc_dv_movie = await _respository.GetByIDAsync(id);

            return View(mbc_dv_movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var model = await _respository.GetByIDAsync(id);

            await _respository.DeleteAsync(model);

            return RedirectToAction(nameof(Index));
        } 
    }
}
