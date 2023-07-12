using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCozaStore.Areas.Admin.Components
{
    [ViewComponent(Name = ("AdminRecentPostView"))]
    public class AdminRecentPostViewComponent : ViewComponent
    {
        private readonly dbCozaStoreContext _context;

        public AdminRecentPostViewComponent(dbCozaStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listOfRecentPost = (from post in _context.TblPosts
                                    where (post.IsActive == true && post.IsHot == true)
                                    orderby post.PostId descending
                                    select post).Take(5).ToList();

            return await Task.FromResult((IViewComponentResult)View("Default", listOfRecentPost));
        }

        
    }
}
