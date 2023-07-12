using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace eCozaStore.Components
{
    [ViewComponent(Name = "SliderView")]
    public class SliderViewComponent : ViewComponent
    {
        private dbCozaStoreContext _dbCozaStoreContext;
        public SliderViewComponent(dbCozaStoreContext dbCozaStoreContext)
        {
            _dbCozaStoreContext = dbCozaStoreContext;
        }
        public IViewComponentResult Invoke()
        {
            var listOfMenu = (from slider in _dbCozaStoreContext.TblSliders
                              where (slider.IsActive == true) && (slider.Position == 1)
                              select slider).ToList();
            return View("Default", listOfMenu);
        }
    }
}
