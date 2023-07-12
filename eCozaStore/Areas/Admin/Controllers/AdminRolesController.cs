using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminRolesController : Controller
    {
        private readonly dbCozaStoreContext _context;

        public AdminRolesController(dbCozaStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int RoleID = 0)
        {
            // Phân trang 
            var pageNumber = page;
            var pageSize = 10;

            List<TblRole> lsRoles = new List<TblRole>();

            if (RoleID != 0)
            {
                lsRoles = _context.TblRoles
                    .AsNoTracking()
                    .Where(x => x.RoleId == RoleID)
                    .OrderByDescending(x => x.RoleId).ToList();
            }
            else
            {
                lsRoles = _context.TblRoles
                    .AsNoTracking()
                    .OrderByDescending(x => x.RoleId).ToList();
            }

            PagedList<TblRole> models = new PagedList<TblRole>(lsRoles.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentRoleID = RoleID;
            ViewBag.CurrentPage = pageNumber;


            // Lọc quyền
            var RoleList = (from role in _context.TblRoles
                                select new SelectListItem()
                                {
                                    Text = role.RoleName,
                                    Value = role.RoleId.ToString(),
                                }).ToList();
            RoleList.Insert(0, new SelectListItem()
            {
                Text = "----Chọn quyền ----",
                Value = "0"
            });

            ViewBag.RoleList = RoleList;

            return View(models);
        }

        // Lọc quyền
        public IActionResult Filtter(int RoleID = 0)
        {
            var url = $"/Admin/AdminRoles?RoleId={RoleID}";
            if (RoleID == 0)
            {
                url = $"/Admin/AdminRoles";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // Details: 
        public IActionResult Details(int? RoleID)
        {
            if(RoleID == null || RoleID == 0)
            {
                return NotFound();
            }

            var tblRole = _context.TblRoles.Find(RoleID);

            if(tblRole == null)
            {
                return NotFound();
            }

            return View(tblRole);

        } 


        // Thêm mới quyền truy cập
         public IActionResult Create()
         {
            return View();
         }

         [HttpPost] 
         public IActionResult Create(TblRole tblRole)
         {
            if(ModelState.IsValid)
            {
                _context.Add(tblRole);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(tblRole);
         }



        // Chỉnh sửa quyền truy cập

        public IActionResult Edit(int? RoleID)
        {
            if(RoleID == null || RoleID == 0)
            {
                return NotFound();
            }

            var tblRole = _context.TblRoles.Find(RoleID);
            if(tblRole == null)
            {
                return NotFound();
            }

            return View(tblRole);
        }

        [HttpPost]
        public IActionResult Edit(TblRole tblRole)
        {
            if(ModelState.IsValid)
            {
                 _context.Update(tblRole);
                 _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblRole);
        }

        // Xóa quyền truy cập

        public IActionResult Delete(int? RoleID)
        {
            if(RoleID == null || RoleID == 0)
            {
                return NotFound();
            }

            var tblRole = _context.TblRoles.Find(RoleID);

            if(tblRole == null)
            {
                return NotFound();
            }

            return View(tblRole);
        }

        [HttpPost]
         public IActionResult Delete(int RoleID)
         {
            var tblRole = _context.TblRoles.Find(RoleID);
            if(tblRole == null)
            {
                return NotFound();
            }
            _context.TblRoles.Remove(tblRole);
            _context.SaveChanges();

            return RedirectToAction("Index");
         }

    }
}
