using eCozaStore.Models;
using eCozaStore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMenusController : Controller
    {
        private readonly dbCozaStoreContext _context;

        public AdminMenusController(dbCozaStoreContext context)
        {
            _context = context;
        }



        public IActionResult Index(int page = 1, int MenuID = 0)
        {
            var pageNumber = page;
            var pageSize = Functions.PAGE_SIZE;



            List<TblMenu> lsMenus = new List<TblMenu>();

            if(MenuID != 0)
            {
                lsMenus = (from mn in _context.TblMenus
                               where mn.MenuId == MenuID
                               orderby mn.MenuId descending
                               select mn).ToList();

            } else
            {
                lsMenus = (from mn in _context.TblMenus
                           orderby mn.MenuId descending
                           select mn).ToList();
            }


            PagedList<TblMenu> models = new PagedList<TblMenu>(lsMenus.AsQueryable(), pageNumber, pageSize);
            
            ViewBag.CurrentMenuID = MenuID;
            ViewBag.CurrentPage = pageNumber;

            // Lấy ra menu

            var MenuList = (from mn in _context.TblMenus
                                select new SelectListItem()
                                {
                                    Text = mn.MenuName,
                                    Value = mn.MenuId.ToString(),
                                }).ToList();
            

            ViewBag.MenuList = MenuList;

            return View(models);
        }
        // Lọc menu

        // PHẦN NÀY ĐỂ LỌC menu
        public IActionResult Filtter(int MenuID = 0)
        {
            var url = $"/Admin/AdminMenus?MenuId={MenuID}";
            if (MenuID == 0)
            {
                url = $"/Admin/AdminMenus";
            }
            return Json(new { status = "success", redirectUrl = url });
        }


        // =========================================

        public IActionResult Details(int? menuID)
        {
            if(menuID == null || menuID == 0)
            {
                return NotFound();

            }

            var tblMenus = _context.TblMenus
                           .Include(x => x.Category)
                           .FirstOrDefault(x => x.MenuId == menuID);

            if(tblMenus == null)
            {
                return NotFound();
            }

            return View(tblMenus);
        }

        // =========================================
        public IActionResult Create()
        {
            // Lấy menu cha
            var MenuList = (from mn in _context.TblMenus

                                select new SelectListItem()
                                {
                                    Text = mn.MenuName,
                                    Value = mn.MenuId.ToString(),
                                }).ToList();
            MenuList.Insert(0, new SelectListItem()
            {
                Text = "----Chọn menu ----",
                Value = "0"
            });

            ViewBag.MenuList = MenuList;


            // Lấy ra danh mục

            var CategoryList = (from cat in _context.TblCategories

                                select new SelectListItem()
                                {
                                    Text = cat.CategoryName,
                                    Value = cat.CategoryId.ToString(),
                                }).ToList();
            CategoryList.Insert(0, new SelectListItem()
            {
                Text = "----Chọn danh mục ----",
                Value = "0"
            });

            ViewBag.CategoryList = CategoryList;


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(TblMenu tblMenus)
        {
            if(ModelState.IsValid)
            {

                tblMenus.CreatedDate = DateTime.Now;

                _context.Add(tblMenus);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(tblMenus);
        }

        // ====================================== 

        public IActionResult Edit(int? menuID)
        {
            if(menuID == null || menuID == 0)
            {
                return NotFound();
            }

            var tblMenus = _context.TblMenus.Find(menuID);

            if(tblMenus == null)
            {
                return NotFound();
            }

            // Lấy menu cha
            var MenuList = (from mn in _context.TblMenus

                            select new SelectListItem()
                            {
                                Text = mn.MenuName,
                                Value = mn.MenuId.ToString(),
                            }).ToList();
            MenuList.Insert(0, new SelectListItem()
            {
                Text = "----Chọn menu ----",
                Value = string.Empty
            });

            ViewBag.MenuList = MenuList;


            // Lấy ra danh mục

            var CategoryList = (from cat in _context.TblCategories

                                select new SelectListItem()
                                {
                                    Text = cat.CategoryName,
                                    Value = cat.CategoryId.ToString(),
                                }).ToList();
            CategoryList.Insert(0, new SelectListItem()
            {
                Text = "----Chọn danh mục ----",
                Value = "0"
            });

            ViewBag.CategoryList = CategoryList;

            return View(tblMenus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(TblMenu tblMenus)
        {
            if(ModelState.IsValid)
            {

                // Lưu lại thời gian sửa
                tblMenus.ModifiedDate = DateTime.Now;

                _context.Update(tblMenus);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblMenus);
        }

        // ==============================================

        public IActionResult Delete(int? menuID)
        {
            if(menuID == null || menuID == 0)
            {
                return NotFound();
            }

            var tblMenus = _context.TblMenus
                           .Include(x => x.Category)
                           .FirstOrDefault(x => x.MenuId == menuID);

            if (tblMenus == null)
            {
                return NotFound();
            }

            return View(tblMenus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int menuID)
        {
            var tblMenus = _context.TblMenus
                            .Include(x => x.Category)
                            .FirstOrDefault(x => x.MenuId == menuID);

            if(tblMenus == null)
            {
                return NotFound();
            }

            _context.TblMenus.Remove(tblMenus);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
