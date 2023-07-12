using eCozaStore.Models;
using eCozaStore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminSlidersController : Controller
    {
        private readonly dbCozaStoreContext _context;

        public AdminSlidersController(dbCozaStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {

            var pageNumber = page;
            var pageSize = Functions.PAGE_SIZE;

            var lsSliders = (from sl in _context.TblSliders
                             orderby sl.SliderId descending
                             select sl).ToPagedList(pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;


            return View(lsSliders);
        }

        public IActionResult Details(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var tblSlider = _context.TblSliders
                            .Include(x => x.category)
                            .FirstOrDefault(x => x.SliderId == id);

            if (tblSlider == null)
            {
                return NotFound();
            }

            return View(tblSlider);
        }

        public IActionResult Create()
        {

            // Lấy ra danh mục

            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TblSlider tblSlider)
        {

            if(ModelState.IsValid)
            {
                

                _context.Add(tblSlider);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }


            // Lấy ra danh mục

            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblSlider.CategoryID);

          
            return View(tblSlider);

        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var tblSlider = _context.TblSliders.Find(id);

            if(tblSlider == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName");
            return View(tblSlider); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(TblSlider tblSlider)
        {
            if(ModelState.IsValid)
            {

              

                _context.TblSliders.Update(tblSlider);
                _context.SaveChanges();

                return RedirectToAction("Index");

            }
            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblSlider.CategoryID);

            return View(tblSlider);
        }


        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();

            }

            var tblSlider = _context.TblSliders
                            .Include(x => x.category)
                            .FirstOrDefault(x => x.SliderId == id);
                           

            if(tblSlider == null)
            {
                return NotFound();
            }

            return View(tblSlider);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var tblSlider = _context.TblSliders
                           .Where(x => x.SliderId == id)
                           .FirstOrDefault();

            if (tblSlider == null)
            {
                return NotFound();
            }

            _context.TblSliders.Remove(tblSlider);
            _context.SaveChanges();

            if (tblSlider.Images != null)
            {
                // Xóa ảnh 
                var fileName = "wwwroot" + tblSlider.Images;
                System.IO.File.Delete(fileName);
            }


            return RedirectToAction("Index");

        }

    }
}
