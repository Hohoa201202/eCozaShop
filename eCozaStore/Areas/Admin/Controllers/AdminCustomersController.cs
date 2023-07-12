using eCozaStore.Models;
using eCozaStore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCustomersController : Controller
    {
        private readonly dbCozaStoreContext _context;
        public AdminCustomersController(dbCozaStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? page = 1)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Functions.PAGE_SIZE;

            var lsCustomers = (from kh in _context.TblCustomers
                               where kh.Active == true
                               orderby kh.CreatedDate descending
                               select kh).ToPagedList(pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;

            return View(lsCustomers);
        }


        public IActionResult Details(int? customerID)
        {
            if (customerID == null || customerID == 0)
            {
                return NotFound();
            }

            var tblCustomer = _context.TblCustomers
                .FirstOrDefault(m => m.CustomerId == customerID);
            if (tblCustomer == null)
            {
                return NotFound();
            }

            return View(tblCustomer);
        }

        public IActionResult hideCustomers(int? id)
        {
            if(id == 0 || id == null)
            {
                return NotFound();
            }

            var tblCustomer = _context.TblCustomers
              .FirstOrDefault(m => m.CustomerId == id);

            if (tblCustomer == null)
            {
                return NotFound();
            }

            return View(tblCustomer);
        }

        [HttpPost]
        public IActionResult hideCustomers(int id)
        {

            var tblCustomer = _context.TblCustomers
              .FirstOrDefault(m => m.CustomerId == id);

            if (tblCustomer == null)
            {
                return NotFound();
            }

            tblCustomer.Active = false;

            _context.TblCustomers.Update(tblCustomer);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
