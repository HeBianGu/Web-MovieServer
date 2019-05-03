using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dms.Product.Base.Model;
using Dms.Product.General.LocalDataBase;
using Dms.Product.Respository.IService;
using System.IO;

namespace Dms.Product.WebApplication.Controllers
{
    public class MovieController : ControllerBase
    {
        private readonly IMovieRespository _respository;

        public MovieController(IMovieRespository respository)
        {
            _respository = respository;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {

            await this.RefreshSeletDropList();

            return View(await _respository.GetListAsync());
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _respository.GetMovieWIthDetial(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
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
        public async Task<IActionResult> Create([Bind("Name,MediaType,TagTypes,AreaType,ExtendType,ArticulationType,Image,VipType,FromType,OrderNum,PlayCount,Score,CDATE,UDATE,ISENBLED,ID")] mbc_dv_movie mbc_dv_movie)
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
        public async Task<IActionResult> Edit(string id, mbc_dv_movie mbc_dv_movie)
        {
            if (id != mbc_dv_movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await _respository.UpdateAsync(mbc_dv_movie);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShootCut(string id)
        {
            var model = await _respository.UpdateImage(id);

            return View(model);
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

        //private bool mbc_dv_movieExists(string id)
        //{
        //    //return _context.mbc_dv_movies.Any(e => e.ID == id);
        //}

        /// <summary> 刷新新建下拉列表 </summary>
        async Task RefreshSeletDropList()
        {

            var customs = await _respository.GetCases();

            Func<mbc_dc_case, string> converto = l =>
            {
                return $"[{l.Name}] {l.BaseUrl} ";
            };

            var caseList = customs.Select(l => Tuple.Create(l.ID, converto(l)));

            ViewBag.Cases = new SelectList(caseList, "Item1", "Item2");
        }


        /// <summary>
        /// 刷新分布视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult RefreshCaseMovie(string id)
        {
            var extends = this._respository.GetExtends();

            List<string> allextends = new List<string>();

            if (extends.Result != null)
            {
                foreach (var item in extends.Result)
                {
                    allextends.AddRange(item.Value.Trim().ToLower().Split('/'));
                }
            }

            Predicate<FileInfo> match = l =>
              {
                  if (allextends.Count == 0) return true;

                  return allextends.Exists(k => k == l.Extension);
              };

            var find = this._respository.GetCases().Result?.FirstOrDefault(l => l.ID == id);

            var result = this._respository.GetListAsync(l => l.CaseType == id);

            bool flag = false;

            if (find != null)
            {
                if (!Directory.Exists(find.BaseUrl))
                {
                    Directory.CreateDirectory(find.BaseUrl);
                }

                Action<FileInfo> action = l =>
                      {
                          if (result != null)
                          {
                              if (result.Result.Exists(k => k.Url == l.FullName)) return;
                          }

                          if (!match(l)) return;

                          flag = true;

                          _respository.Insert(l, id);
                      };

                this.DoAllFiles(find.BaseUrl, action);

                //var files = Directory.GetFiles(find.BaseUrl);

                //foreach (var item in files)
                //{
                //    if (result != null)
                //    {
                //        if (result.Result.Exists(l => l.Url == item)) continue;
                //    }
                //    flag = true;

                //    _respository.Insert(item, id);
                //}

                _respository.SaveAsync();

            }

            if (flag)
            {
                result = this._respository.GetListAsync(l => l.CaseType == id);
            }



            return PartialView("_MovieView", result.Result == null ? new List<mbc_dv_movie>() : result.Result);

        }

        /// <summary>
        /// 刷新分布视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Movie/Edit/RefreshMovieImage")]
        public PartialViewResult RefreshMovieImage(string id)
        { 
            var result = _respository.UpdateImage(id); 

            return PartialView("_MovieImageView", result.Result == null ? new List<string>() : result.Result);

        }

        [HttpPost]
        public PartialViewResult SearchCaseMovie(string id)
        {

            var result = this._respository.GetListAsync(l => l.CaseType == id);

            return PartialView("_MovieView", result.Result == null ? new List<mbc_dv_movie>() : result.Result);
        }

        public void DoAllFiles(string dirs, Action<FileInfo> action)
        {
            //绑定到指定的文件夹目录
            DirectoryInfo dir = new DirectoryInfo(dirs);

            //检索表示当前目录的文件和子目录
            FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();

            //遍历检索的文件和子目录
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                //判断是否为空文件夹　　
                if (fsinfo is DirectoryInfo)
                {
                    //递归调用
                    DoAllFiles(fsinfo.FullName, action);
                }
                else if (fsinfo is FileInfo)
                {
                    action(fsinfo as FileInfo);
                }
            }
        }
    }
}
