using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace eCozaStore.Areas.Admin.Components
{
    [ViewComponent(Name = "AdminBestSellersView")]
    public class AdminBestSellersViewComponent : ViewComponent
    {
        private readonly dbCozaStoreContext _context;

        public AdminBestSellersViewComponent(dbCozaStoreContext context)
        {
           _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lsOfBestSellers = (from sl in _context.ViewBestSeller
                                   where (sl.Active == true && sl.BestSellers == true)
                                   orderby sl.ProductId descending
                                   select sl).Take(6).ToList();

            return await Task.FromResult((IViewComponentResult)View("Default", lsOfBestSellers));
        }
    }
}
