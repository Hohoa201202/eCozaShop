using eCozaStore.Areas.Admin.Models;
using eCozaStore.Extension;
using eCozaStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AdminLoginController : Controller
    {
        private readonly dbCozaStoreContext _context;

        public AdminLoginController(dbCozaStoreContext context)
        {
            _context = context;
        }


        // GET: Admin/Login

        [AllowAnonymous]
        [Route("adminlogin.html", Name = "AdminLogin")]
        public IActionResult Login(string returnUrl = null)
        {
            var accountID = HttpContext.Session.GetString("AccountId");
            if (accountID != null) return RedirectToAction("Index", "Home", new { Area = "Admin" });
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        // POST
        [HttpPost]
        [AllowAnonymous]
        [Route("adminlogin.html", Name = "AdminLogin")]
        public IActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {


                    TblAccount account = _context.TblAccounts
                    .Include(p => p.Role)
                    .SingleOrDefault(p => p.Email.ToLower().Trim() == model.Email.ToLower().Trim());

                    if (account == null)
                    {
                        ViewBag.Error = "Thông tin đăng nhập không đúng";
                        return View(model);
                    }
                    string pass = (model.Password.Trim())/* + account.Salt.Trim()).ToMD5()*/;
                    // + account.Salt.Trim().ToMD5()
                    if (account.Password.Trim() != pass)
                    {
                        ViewBag.Error = "Thông tin đăng nhập không đúng";
                        return View(model);
                    }
                    //đăng nhập thành công

                    //ghi nhận thời gian đăng nhập
                    account.LastLogin = DateTime.Now;
                    _context.Update(account);
                    _context.SaveChangesAsync();


                    var accountID = HttpContext.Session.GetString("AccountId");
                    //identity
                    //luuw seccion M
                    HttpContext.Session.SetString("AccountId", account.AccountId.ToString());

                    //identity
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, account.FullName),
                        new Claim(ClaimTypes.Email, account.Email),
                        new Claim("AccountId", account.AccountId.ToString()),
                        new Claim("RoleId", account.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, account.Role.RoleName),
                        new Claim("Email", account.Email),
                        //new Claim("Phone", account.Phone),
                        //new Claim("ID", account.AccountId.ToString()),
                        new Claim("Avt", account.Thumb)
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    HttpContext.SignInAsync(userPrincipal);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "AdminLogin", new { Area = "Admin" });
            }
            return RedirectToAction("Login", "AdminLogin", new { Area = "Admin" });
        }


        // Đăng xuất

        [Route("logout.html", Name = "Logout")]

        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId");
                return RedirectToAction("Login", "AdminLogin", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("Login", "AdminLogin", new { Area = "Admin" });
            }
        }

    }
}
