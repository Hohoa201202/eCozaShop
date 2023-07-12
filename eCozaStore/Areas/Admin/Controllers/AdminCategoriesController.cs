using eCozaStore.Models;
using eCozaStore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCategoriesController : Controller
    {
        private readonly dbCozaStoreContext _context;
        public AdminCategoriesController(dbCozaStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int CategoryID = 0)
        {

            var pageNumber = page;
            var pageSize = Functions.PAGE_SIZE;

            List<TblCategory> lsCategory = new List<TblCategory>();
          
            if(CategoryID != 0)
            {
                lsCategory = (from cat in _context.TblCategories
                              where cat.CategoryId == CategoryID
                              orderby cat.CategoryId descending
                              select cat).ToList();
            }
            else
            {
                lsCategory = (from cat in _context.TblCategories
                              orderby cat.CategoryId descending
                              select cat).ToList();
            }
            PagedList<TblCategory> models = new PagedList<TblCategory>(lsCategory.AsQueryable(), pageNumber, pageSize);
            
            ViewBag.CurrentCategoryID = CategoryID;
            ViewBag.CurrentPage = pageNumber;

            // Lấy ra danh mục

            var CategoryList = (from cat in _context.TblCategories
                            select new SelectListItem()
                            {
                                Text = cat.CategoryName,
                                Value = cat.CategoryId.ToString(),
                            }).ToList();


            ViewBag.CategoryList = CategoryList;

            return View(models);
        }

        // LỌC DANH MỤC
        public IActionResult Filtter(int CategoryID = 0)
        {
            var url = $"/Admin/AdminCategories?CategoryId={CategoryID}";
            if (CategoryID == 0)
            {
                url = $"/Admin/AdminCategories";
            }
            return Json(new { status = "success", redirectUrl = url });
        }



        // Xem chi tiết
        public IActionResult Details(int? categoryID)
        {
            if (categoryID == null || categoryID == 0)
            {
                return NotFound();
            }

            var tblCategory = _context.TblCategories.Find(categoryID);

            if (tblCategory == null)
            {
                return NotFound();
            }

            return View(tblCategory);
        }

        // Thêm mới 

        public IActionResult Create()
        {

            ViewData["CategoryList"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblCategory tblCategory)
        {
            if (ModelState.IsValid)
            {


                _context.TblCategories.Add(tblCategory);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblCategory);
        }


        // Chỉnh sửa

        public IActionResult Edit(int? categoryID)
        {
            if (categoryID == null || categoryID == 0)
            {
                return NotFound();
            }

            var tblCategory = _context.TblCategories.Find(categoryID);

            if (tblCategory == null)
            {
                return NotFound();
            }

            // lấy list danh mục cha

            var CategoryList = (from cat in _context.TblCategories
                                select new SelectListItem()
                                {
                                    Text = cat.CategoryName,
                                    Value = cat.CategoryId.ToString(),
                                }).ToList();
            CategoryList.Insert(0, new SelectListItem()
            {
                Text = "----Chọn danh mục cha----",
                Value = string.Empty
            });

            ViewBag.CategoryList = CategoryList;

            return View(tblCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TblCategory tblCategory)
        {
            if (ModelState.IsValid)
            {

                _context.Update(tblCategory);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(tblCategory);
        }


        // Xóa danh mục

        public IActionResult Delete(int? categoryID)
        {
            if (categoryID == null || categoryID == 0)
            {
                return NotFound();
            }

            var tblCategory = _context.TblCategories.Find(categoryID);

            if (tblCategory == null)
            {
                return NotFound();
            }

            return View(tblCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int categoryID)
        {


            var tblCategory = _context.TblCategories.Find(categoryID);

            if (tblCategory == null)
            {
                return NotFound();
            }

            tblCategory.IsActive = false;

            _context.TblCategories.Update(tblCategory);
            _context.SaveChanges();

            //if (tblCategory.Thumb != "default.jpg")
            //{
            //    // Xóa ảnh cũ
            //    var fileName = "wwwroot" + tblCategory.Thumb;
            //    System.IO.File.Delete(fileName);
            //}

            return RedirectToAction("Index");
        }
    }
}
