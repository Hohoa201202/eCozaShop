using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "FeedbackView")]
    public class FeedbackViewComponents : ViewComponent
    {
        private dbCozaStoreContext _context;
        public FeedbackViewComponents(dbCozaStoreContext dbCozaStoreContext)
        {
            _context = dbCozaStoreContext;
        }

        public IViewComponentResult Invoke(int productID)
        {
            var list = (from p in _context.ViewFeedbacks
                                where (p.IsActive == true) && (p.ProductId == productID) &&(p.Status == true)
                                orderby p.CreatedDate descending
                                select p).ToList();
            return View("Default", list);
        }
    }
}

