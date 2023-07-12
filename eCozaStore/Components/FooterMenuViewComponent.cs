using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using eCozaStore.Models;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "FooterMenuView")]
    public class FooterMenuViewComponent : ViewComponent
    {
        private dbCozaStoreContext _dbCozaStoreContext;

        public FooterMenuViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _dbCozaStoreContext = dbCozaStoreContext;
        }

        public IViewComponentResult Invoke()
        {
            var listOfMenu = (from menu in _dbCozaStoreContext.TblMenus
                              where (menu.IsActive == true) //&& (menu.Position == 1)
                              select menu).ToList();
            return View("Default", listOfMenu);
        }
    }
}
