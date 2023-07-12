using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "OrderDetailView")]
    public class OrderDetailViewComponent : ViewComponent
    {
        private dbCozaStoreContext _context;
        public OrderDetailViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _context = dbCozaStoreContext;
        }

        public IViewComponentResult Invoke(int OrderID)
        {
            var listOfMenu = (from p in _context.ViewOrderDetails
                              where (p.OrderId == OrderID)
                              orderby p.OrderDetailsId descending
                              select p).ToList();
            return View("Default", listOfMenu);
        }
    }
}

