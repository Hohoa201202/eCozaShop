using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using eCozaStore.Models;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "ProductView")]
    public class ProductViewComponent : ViewComponent
    {
        private dbCozaStoreContext _dbCozaStoreContext;

        public ProductViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _dbCozaStoreContext = dbCozaStoreContext;
        }

        public IViewComponentResult Invoke()
        {
            var listOfMenu = (from p in _dbCozaStoreContext.TblProducts
                              where (p.Active == true) && (p.HomeFlag == true)
                              orderby p.ProductId descending
                              select p).ToList();
            return View("Default", listOfMenu);
        }
    }
}