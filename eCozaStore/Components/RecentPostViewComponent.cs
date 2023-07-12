using Microsoft.AspNetCore.Mvc;
using eCozaStore.Models;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "RecentPostView")]
    public class RecentPostViewComponent : ViewComponent
    {
        private readonly dbCozaStoreContext _dbCozaStoreContext;

        public RecentPostViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _dbCozaStoreContext = dbCozaStoreContext;
        }

        public IViewComponentResult Invoke()
        {
            var listOfRecentPost = (from p in _dbCozaStoreContext.TblPosts
                                    where (p.IsActive == true)
                                    orderby p.CreatedDate descending
                                    select p).Take(5).ToList();
            return View("Default", listOfRecentPost);
        }
    }
}
