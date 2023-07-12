using Microsoft.AspNetCore.Mvc;
using eCozaStore.Models;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "FeaturedProductsView")]
    public class FeaturedProductsViewComponent : ViewComponent
    {
        private dbCozaStoreContext _context;
        public FeaturedProductsViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _context = dbCozaStoreContext;
        }

        public IViewComponentResult Invoke()
        {
            var listOfProduct = (from p in _context.TblProducts
                                 where (p.Active == true) && (p.BestSellers == true)
                                 orderby p.Discount descending
                                 select p).Take(3).ToList();
            return View("Default", listOfProduct);
        }
    }
}
