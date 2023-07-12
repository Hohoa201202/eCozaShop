using eCozaStore.Models;
using eCozaStore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminOrderDetailsController : Controller
    {
        private readonly dbCozaStoreContext _context;

        public AdminOrderDetailsController(dbCozaStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int kh = 0, int tt = 0)
        {
            var pageNumber = page;
            var pageSize = Functions.PAGE_SIZE;


            var lsOrders = (from o in _context.ViewOrders
                            orderby o.OrderDate descending
                            select o).ToPagedList(pageNumber, pageSize);


            ViewBag.CurrentPage = pageNumber;

            return View(lsOrders);
        }

        public IActionResult Details(int? id)
        {
            if(id == 0 || id == null)
            {
                return NotFound();
            }

            var lsOrders = _context.TblOrders
                            .Include(x => x.Customer)
                            .Include(y => y.TransactStatus)
                            .FirstOrDefault(x => x.OrderId == id);

            if(lsOrders == null)
            {
                return NotFound();
            }

            var lsOrderDetails = _context.TblOrderDetails
                                .Include(x => x.Product)
                                .Where(x => x.OrderId == lsOrders.OrderId)
                                .OrderBy(x => x.OrderDetailsId)
                                .ToList();

            ViewBag.OrderDetails = lsOrderDetails;

            return View(lsOrders);

        }



        public IActionResult ChangeStatus(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var lsOrders = _context.TblOrders
                            .Include(x => x.Customer)
                            .Include(y => y.TransactStatus)
                            .FirstOrDefault(x => x.OrderId == id);


            if (lsOrders == null)
            {
                return NotFound();
            }
         
            ViewData["OrderStatus"] = new SelectList(_context.TblTransactStatuses, "TransactStatusId", "Status", lsOrders.TransactStatusId);

            return View(lsOrders);

        }

        [HttpPost]

        public IActionResult ChangeStatus(TblOrder tblOrder, int id)
        {
            if(ModelState.IsValid)
            {
                var lsOrders = _context.TblOrders
                               .Include(x => x.Customer)
                               .Include(y => y.TransactStatus)
                               .FirstOrDefault(x => x.OrderId == id);

                if(lsOrders != null)
                {
                    lsOrders.Paid = tblOrder.Paid;
                    lsOrders.Deleted = tblOrder.Deleted;
                    lsOrders.TransactStatusId = tblOrder.TransactStatusId;

                    if(lsOrders.Paid == true)
                    {
                        lsOrders.PaymentDate = DateTime.Now;
                    }

                    if(lsOrders.TransactStatusId == 5)
                    {
                        lsOrders.Deleted = true;
                    }

                    if (lsOrders.TransactStatusId == 4)
                    {
                        lsOrders.Deleted = false;
                        lsOrders.Paid = true;
                    }

                    if (lsOrders.TransactStatusId == 3)
                    {
                        lsOrders.ShipDate = DateTime.Now;
                    }
                }

                _context.Update(lsOrders);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["OrderStatus"] = new SelectList(_context.TblTransactStatuses, "TransactStatusId", "Status", tblOrder.TransactStatusId);
            return View();
        }
    }
}
