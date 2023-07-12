using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly dbCozaStoreContext _context;

        public SearchController(dbCozaStoreContext context) {
            _context = context;
        }
        [HttpPost]

        // Tìm kiếm sản phẩm
        public IActionResult FindProduct(string keyword)
        {
            List<TblProduct> ls = new List<TblProduct>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                var list = _context.TblProducts.AsNoTracking()
                                  .Include(a => a.Category)
                                  .OrderByDescending(x => x.ProductName)
                                  .Take(10)
                                  .ToList();
                return PartialView("ListProductsSearchPartial", list);
            }


            ls = _context.TblProducts.AsNoTracking()
                                  .Include(a => a.Category)
                                  .Where(x => x.ProductName.Contains(keyword))
                                  .OrderByDescending(x => x.ProductName)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartial", ls);
            }
        }


        // Tìm kiếm danh mục
        public IActionResult FindCategories(string keyword)
        {
            List<TblCategory> ls = new List<TblCategory>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                var list = _context.TblCategories.AsNoTracking()
                                  .OrderByDescending(x => x.CategoryName)
                                  .Take(10)
                                  .ToList();
                return PartialView("ListCategoriesSearchPartial", list);
            }


            ls = _context.TblCategories.AsNoTracking()
                                  .Where(x => x.CategoryName.Contains(keyword))
                                  .OrderByDescending(x => x.CategoryName)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListCategoriesSearchPartial", null);
            }
            else
            {
                return PartialView("ListCategoriesSearchPartial", ls);
            }
        }

        // Tìm kiếm khách hàng
        public IActionResult FindCustomers(string keyword)
        {
            List<TblCustomer> ls = new List<TblCustomer>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                var list = _context.TblCustomers.AsNoTracking()
                                  .OrderByDescending(x => x.FullName)
                                  .Take(10)
                                  .ToList();

                return PartialView("ListCustomersSearchPartial", list);
            }


            ls = _context.TblCustomers.AsNoTracking()
                                  .Where(x => x.FullName.Contains(keyword))
                                  .OrderByDescending(x => x.FullName)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListCustomersSearchPartial", null);
            }
            else
            {
                return PartialView("ListCustomersSearchPartial", ls);
            }
        }




        // Tìm kiếm menu
        public IActionResult FindMenus(string keyword)
        {
            List<TblMenu> ls = new List<TblMenu>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                var list  = _context.TblMenus.AsNoTracking()
                                  .OrderByDescending(x => x.MenuName)
                                  .Take(10)
                                  .ToList();
                return PartialView("ListMenusSearchPartial", list);
            }


            ls = _context.TblMenus.AsNoTracking()
                                  .Where(x => x.MenuName.Contains(keyword))
                                  .OrderByDescending(x => x.MenuName)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListMenusSearchPartial", null);
            }
            else
            {
                return PartialView("ListMenusSearchPartial", ls);
            }
        }

        // Tìm kiếm tài khoản
        public IActionResult FindAccounts(string keyword)
        {
            List<TblAccount> ls = new List<TblAccount>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                var list = _context.TblAccounts.AsNoTracking()
                                  .Include(x => x.Role)
                                  .OrderByDescending(x => x.FullName)
                                  .Take(10)
                                  .ToList();
                return PartialView("ListAccountsSearchPartial", list);
            }


            ls = _context.TblAccounts.AsNoTracking()
                                  .Include(x => x.Role)
                                  .Where(x => x.FullName.Contains(keyword))
                                  .OrderByDescending(x => x.FullName)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListAccountsSearchPartial", null);
            }
            else
            {
                return PartialView("ListAccountsSearchPartial", ls);
            }
        }


        // Tìm kiếm tin tức
        public IActionResult FindPosts(string keyword)
        {
            List<TblPost> ls = new List<TblPost>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                var list = _context.TblPosts.AsNoTracking()
                                  .Include(x => x.Category)
                                  .OrderByDescending(x => x.Title)
                                  .Take(10)
                                  .ToList();

                return PartialView("ListPostsSearchPartial", list);
            }


            ls = _context.TblPosts.AsNoTracking()
                                  .Include(x => x.Category)
                                  .Where(x => x.Title.Contains(keyword))
                                  .OrderByDescending(x => x.Title)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListPostsSearchPartial", null);
            }
            else
            {
                return PartialView("ListPostsSearchPartial", ls);
            }
        }
    }
}
