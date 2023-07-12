using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace eCozaStore.Controllers
{
    public class SearchController : Controller
    {
        private readonly dbCozaStoreContext _context;

        public SearchController(dbCozaStoreContext context)
        {
            _context = context;
        }
        [HttpPost]

        // Tìm kiếm sản phẩm
        public IActionResult SearchProduct(string inputName, int inputCateID)
        {
            if (string.IsNullOrEmpty(inputName) || inputName.Length < 1)
            {
                var lsDefault = (from item in _context.TblProducts
                                 where (item.CategoryId == inputCateID)
                                 orderby (item.ProductName)
                                 select item).Take(16).ToList();

                return PartialView("ListSearchProducts", lsDefault);
            }

            var ls = (from item in _context.TblProducts
                      where (item.ProductName.Contains(inputName) && (item.CategoryId == inputCateID))
                      orderby (item.ProductName)
                      select item).Take(16).ToList();

            if (ls == null)
            {
                return PartialView("ListSearchProducts", null);
            }
            else
            {
                return PartialView("ListSearchProducts", ls);
            }
        }
    }
}
