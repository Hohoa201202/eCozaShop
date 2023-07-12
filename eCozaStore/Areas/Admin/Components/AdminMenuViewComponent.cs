using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCozaStore.Areas.Admin.Components
{
    [ViewComponent(Name ="AdminMenuView")]
    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly dbCozaStoreContext _context;

        public AdminMenuViewComponent(dbCozaStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mnList = (from mn in _context.TblAdminMenus
                          where (mn.IsActive == true)
                          select mn).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", mnList));
        }
    }
}
