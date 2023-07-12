using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "HotCategoryView")]
    public class HotCategoryViewComponent : ViewComponent
    {
        private readonly dbCozaStoreContext _dbCozaStoreContext;
        public HotCategoryViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _dbCozaStoreContext = dbCozaStoreContext;
        }
        public IViewComponentResult Invoke()
        {
            var listOfCategory = (from category in _dbCozaStoreContext.TblCategories
                                  where category.IsActive == true
                                  select category).ToList();
            return View("Default", listOfCategory);
        }
    }
}
