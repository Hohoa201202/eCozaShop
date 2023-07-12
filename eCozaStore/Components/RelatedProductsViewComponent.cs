
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using eCozaStore.Models;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "RelatedProductsView")]
    public class RelatedProductsViewComponent : ViewComponent
    {
        private dbCozaStoreContext _dbCozaStoreContext;

        public RelatedProductsViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _dbCozaStoreContext = dbCozaStoreContext;
        }

        public IViewComponentResult Invoke(int CateID)
        {
            var listOfMenu = (from p in _dbCozaStoreContext.TblProducts
                              where (p.Active == true) && (p.CategoryId == CateID)
                              orderby p.Discount descending
                              select p).Take(8).ToList();
            return View("Default", listOfMenu);
        }
    }
}
