using Microsoft.AspNetCore.Mvc;
using eCozaStore.Models;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "PostView")]
    public class PostViewComponent : ViewComponent
    {
        private readonly dbCozaStoreContext _dbCozaStoreContext;
        public PostViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _dbCozaStoreContext = dbCozaStoreContext;
        }

        public IViewComponentResult Invoke()
        {
            var listOfPostMenu = (from p in _dbCozaStoreContext.ViewPostCats
                                  where (p.IsActive == true)
                                  select p
                                  ).ToList();
            return View("Default", listOfPostMenu);
        }
    }
}
