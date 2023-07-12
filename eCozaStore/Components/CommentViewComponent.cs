using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "CommentView")]
    public class CommentViewComponent : ViewComponent
    {
        private dbCozaStoreContext _context;
        public CommentViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _context = dbCozaStoreContext;
        }

        public IViewComponentResult Invoke(int postID, int page)
        {
            var pageNumber = page;
            var pageSize = 10;

            var listComments = (from p in _context.TblComments
                               join c in _context.TblCustomers on p.CustomerId equals c.CustomerId into t
                               from c in t.DefaultIfEmpty()
                               where (p.IsActive == true) && (p.PostId == postID)
                               orderby p.CreatedDate descending
                               select new ViewComment
                               {
                                   CommentId = p.CommentId,
                                   PostId = p.PostId,
                                   CustomerId = p.CustomerId,
                                   ParentId = p.ParentId,
                                   Content = p.Content,
                                   CreatedDate = p.CreatedDate,
                                   IsActive = p.IsActive,
                                   Author = p.Author,
                                   Slike = p.Slike,
                                   Avatar = c.Avatar,
                                   FullName = c.FullName,
                                   Thumb = p.Thumb
                               });
            
            ViewBag.postID = postID;
            ViewBag.sComment = listComments.Count();
            return View("Default", listComments.ToPagedList(pageNumber, pageSize));
        }
    }
}

