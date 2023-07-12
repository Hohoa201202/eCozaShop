using eCozaStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // phải xác thực đăng nhập mới vào đc home
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbCozaStoreContext _dbCozaStoreContext;

        public HomeController(ILogger<HomeController> logger, dbCozaStoreContext dbCozaStoreContext)
        {
            _logger = logger;
            _dbCozaStoreContext = dbCozaStoreContext;
        }
        public IActionResult Index()
        {
            var AccountID = HttpContext.Session.GetString("AccID");
            if (AccountID != null)
            {
                HttpContext.SignOutAsync();

                var AccID = HttpContext.Session.GetString("AccountId");

                if (AccID != null)
                {
                    var data = _dbCozaStoreContext.TblAccounts.Find(int.Parse(AccID));
                    //Identity
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, data.FullName),
                        new Claim(ClaimTypes.Email, data.Email),
                        new Claim("AccountId", data.AccountId.ToString()),
                        new Claim("RoleId", data.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, data.Role.RoleName),
                        new Claim("Email", data.Email),
                        //new Claim("Phone", account.Phone),
                        //new Claim("ID", account.AccountId.ToString()),
                        new Claim("Avt", data.Thumb)
                    };

                    var gran = new ClaimsIdentity(userClaims, "User");
                    var userPr = new ClaimsPrincipal(new[] { gran });
                    HttpContext.SignInAsync(userPr);
                }
            }
            return View();
        }

    }
}
