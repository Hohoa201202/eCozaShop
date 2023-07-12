using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using eCozaStore.Models;
using eCozaStore.Controllers;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "MenuView")]
    public class MenuViewComponent : ViewComponent
    {
        private dbCozaStoreContext _dbCozaStoreContext;

        public MenuViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _dbCozaStoreContext = dbCozaStoreContext;
        }

        public IViewComponentResult Invoke()
        {
            var listOfMenu = (from menu in _dbCozaStoreContext.TblMenus
                              where (menu.IsActive == true) //&& (menu.Position == 1)
                              select menu).ToList();
            ViewBag.Notify = "data-notify=" + CartController.notify;
            return View("Default", listOfMenu);
        }
    }
}
