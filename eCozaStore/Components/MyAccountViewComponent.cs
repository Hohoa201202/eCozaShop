using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "MyAccountView")]
    public class MyAccountViewComponent : ViewComponent
    {
        private dbCozaStoreContext _context;
        public MyAccountViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _context = dbCozaStoreContext;
        }

        public IViewComponentResult Invoke()
        {
            return View("Default");
        }
    }
}
