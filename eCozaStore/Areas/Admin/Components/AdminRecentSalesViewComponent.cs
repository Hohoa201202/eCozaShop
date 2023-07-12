using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCozaStore.Areas.Admin.Components
{
    [ViewComponent(Name = ("AdminRecentSalesView"))]
    public class AdminRecentSalesViewComponent : ViewComponent
    {
        private readonly dbCozaStoreContext _context;

        public AdminRecentSalesViewComponent(dbCozaStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listOfRecentPost = (from s in _context.ViewOrders
                                    where (s.Active == true)
                                    orderby s.ShipDate descending
                                    select s).Take(5).ToList();

            return await Task.FromResult((IViewComponentResult)View("Default", listOfRecentPost));
        }

    }
}