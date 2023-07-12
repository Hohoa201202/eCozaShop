using eCozaStore.Areas.Admin.Models;
using eCozaStore.Extension;
using eCozaStore.Models;
using eCozaStore.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System.Security.Principal;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountsController : Controller
    {
        private readonly dbCozaStoreContext _context;

        public AdminAccountsController(dbCozaStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int RoleID = 0)
        {
            var pageNumber = page;
            var pageSize = Functions.PAGE_SIZE;

            List<TblAccount> lsAccounts = new List<TblAccount>();

            if (RoleID != 0)
            {
                lsAccounts = (from ac in _context.TblAccounts
                              join rl in _context.TblRoles on ac.RoleId equals rl.RoleId
                              where ac.RoleId == RoleID
                              orderby ac.AccountId descending
                              select ac).ToList();
            }
            else
            {
                lsAccounts = (from ac in _context.TblAccounts
                              join rl in _context.TblRoles on ac.RoleId equals rl.RoleId
                              orderby ac.AccountId descending
                              select ac).ToList();
            }

            PagedList<TblAccount> lsModels = new PagedList<TblAccount>(lsAccounts.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentRoleID = RoleID;
            ViewBag.CurrentPage = pageNumber;

            ViewData["QuyenTruyCap"] = new SelectList(_context.TblRoles, "RoleId", "RoleName");

            return View(lsModels);
        }

        // Lọc quyền
        public IActionResult Filtter(int RoleID = 0)
        {
            var url = $"/Admin/AdminAccounts?RoleId={RoleID}";
            if (RoleID == 0)
            {
                url = $"/Admin/AdminAccounts";
            }
            return Json(new { status = "success", redirectUrl = url });
        }


        // ===============================================================================
        public IActionResult Details(int? accountID)
        {
            if (accountID == null || accountID == 0)
            {
                return NotFound();
            }

            var tblAccounts = _context.TblAccounts
                            .Include(x => x.Role)
                            .FirstOrDefault(x => x.AccountId == accountID);

            if (tblAccounts == null)
            {
                return NotFound();
            }

            return View(tblAccounts);
        }


        // ====================================================================================

        public IActionResult Create()
        {
            // Lấy danh sách quyền truy cập
            var RoleList = (from role in _context.TblRoles
                            select new SelectListItem()
                            {
                                Text = role.RoleName,
                                Value = role.RoleId.ToString(),
                            }).ToList();
            RoleList.Insert(0, new SelectListItem()
            {
                Text = "----Chọn quyền ----",
                Value = "0"
            });

            ViewBag.RoleList = RoleList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblAccount model, IFormFile? fThumb)
        {
            if (ModelState.IsValid)
            {
                // Tạo ngẫu nhiên mật khẩu
                string salt = Functions.GetRandomKey();

                model.Salt = salt;
                model.Password = (model.Phone.Trim() + salt.Trim()).ToMD5();
                model.CreatedDate = DateTime.Now;

                // Tạo hình ảnh
                // xử lý Thumb 

                model.FullName = Functions.ToTitleCase(model.FullName);


                if (fThumb != null) // nếu có chọn hình ảnh
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Functions.SEOUrl(model.FullName + model.AccountId.ToString()) + extension;
                    model.Thumb = await Functions.UploadFile(fThumb, @"posts", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(model.Thumb)) model.Thumb = "default.jpg"; // 


                _context.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ================================================================

        public IActionResult Edit(int? accountID)
        {
            if (accountID == null || accountID == 0)
            {
                return NotFound();

            }

            var tblAccounts = _context.TblAccounts.Include(x => x.Role).FirstOrDefault(x => x.AccountId == accountID);

            if (tblAccounts == null)
            {
                return NotFound();
            }

            var RoleList = (from role in _context.TblRoles
                            select new SelectListItem()
                            {
                                Text = role.RoleName,
                                Value = role.RoleId.ToString(),
                            }).ToList();
            RoleList.Insert(0, new SelectListItem()
            {
                Text = "----Chọn quyền ----",
                Value = "0"
            });

            ViewBag.RoleList = RoleList;

            return View(tblAccounts);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TblAccount tblAccounts)
        {
            if (ModelState.IsValid)
            {


                // Tạo ngẫu nhiên laị mật khẩu trong database
                string salt = Functions.GetRandomKey();

                tblAccounts.Salt = salt;
                tblAccounts.Password = (tblAccounts.Phone.Trim() + salt.Trim()).ToMD5(); // lấy lại số điện thoại làm mật khẩu

                _context.Update(tblAccounts);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(tblAccounts);
        }

        // =========================================================

        public IActionResult Delete(int? accountID)
        {
            if (accountID == null || accountID == 0)
            {
                return NotFound();
            }

            var tblAccounts = _context.TblAccounts.Find(accountID);

            if (tblAccounts == null)
            {
                return NotFound();
            }

            var RoleList = (from role in _context.TblRoles
                            select new SelectListItem()
                            {
                                Text = role.RoleName,
                                Value = role.RoleId.ToString(),
                            }).ToList();
            RoleList.Insert(0, new SelectListItem()
            {
                Text = "----Chọn quyền ----",
                Value = "0"
            });

            ViewBag.RoleList = RoleList;

            return View(tblAccounts);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int accountID)
        {
            var tblAccounts = _context.TblAccounts.Find(accountID);

            if (tblAccounts == null)
            {
                return NotFound();
            }

            _context.TblAccounts.Remove(tblAccounts);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // ============================================ 


        // Đổi mật khẩu
        [Route("doi-mat-khau.html", Name = "ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            string AccountID = HttpContext.Session.GetString("AccountId");

            if (AccountID == null)
            {
                //Remove session
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccID");

                return RedirectToAction("Login", "AdminLogin", new { Area = "Admin" });
            }

            if (ModelState.IsValid)
            {
                var acc = _context.TblAccounts.AsNoTracking().FirstOrDefault(a => a.AccountId == int.Parse(AccountID));

                if (acc == null)
                {
                    return NotFound();
                }

                try
                {
                    //Lấy pass cũ để kiểm tra
                    string passNow = (model.PasswordNow.Trim() + acc.Salt.Trim()).ToMD5();

                    if (passNow == acc.Password.Trim()) //Đúng pass
                    {
                        //Tạo pass mới
                        acc.Password = (model.Password.Trim() + acc.Salt.Trim()).ToMD5();
                        _context.TblAccounts.Update(acc);
                        _context.SaveChanges();

                        ViewBag.Error = "Đổi mật khẩu thành công";
                        return RedirectToAction("EditProfile", "AdminAccounts", new { Area = "Admin", alter = ViewBag.Error });
                    }
                    else
                    {
                        ViewBag.Error = "Mật cũ không đúng";
                        return RedirectToAction("EditProfile", "AdminAccounts", new { Area = "Admin", alter = ViewBag.Error });
                    }
                }
                catch
                {
                    return NotFound();
                }
            }
            return NotFound();
        }


        // Chỉnh sửa profile

        [Route("edit-profile.html", Name = "EditProfile")]
        [Authorize, HttpGet]

        public IActionResult EditProfile(string? alter)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/login.html");

            var accountID = HttpContext.Session.GetString("AccountId");
            if (accountID == null) return RedirectToAction("Login", "AdminLogin", new { Area = "Admin" });

            var account = _context.TblAccounts
                        .AsNoTracking()
                        .Include(x => x.Role)
                        .FirstOrDefault(x => x.AccountId == int.Parse(accountID));
            if (account == null) return RedirectToAction("Login", "AdminLogin", new { Area = "Admin" });

            if(alter != null)
            {
                ViewBag.Alter = alter;
            }

            return View(account);

        }


        [Route("edit-profile.html", Name = "EditProfile")]
        [Authorize, HttpPost]
        public async Task<IActionResult> EditProfile(TblAccount model, IFormFile? fThumb)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/login.html");

            var accountID = HttpContext.Session.GetString("AccountId");
            if (accountID == null) return RedirectToAction("Login", "AdminLogin", new { Area = "Admin" });

            if (ModelState.IsValid)
            {

                // xử lý Thumb 
                model.FullName = Functions.ToTitleCase(model.FullName);

                if (fThumb != null) // nếu có chọn hình ảnh
                {
                    if (model.Thumb == "default.jpg")
                    {
                        // Tạo hình ảnh mới
                        string extension = Path.GetExtension(fThumb.FileName);
                        string imageName = Functions.SEOUrl(model.FullName) + extension;
                        model.Thumb = await Functions.UploadFile(fThumb, @"account", imageName.ToLower());
                    }

                    else
                    {
                        // Xóa ảnh cũ
                        var fileName = "wwwroot/files/images/account/" + model.Thumb;
                        System.IO.File.Delete(fileName);

                        //Tạo hình ảnh mới
                        string extension = Path.GetExtension(fThumb.FileName);
                        string imageName = Functions.SEOUrl(model.FullName) + extension;
                        model.Thumb = await Functions.UploadFile(fThumb, @"account", imageName.ToLower());
                    }

                }
                if (string.IsNullOrEmpty(model.Thumb)) model.Thumb = "default.jpg";

                var account = _context.TblAccounts
                                   .AsNoTracking()
                                   .Include(x => x.Role)
                                   .FirstOrDefault(x => x.AccountId == int.Parse(accountID));
                try
                {
                    account.FullName = model.FullName;
                    account.Phone = model.Phone;
                    account.Email = model.Email;
                    account.Address = model.Address;
                    account.BirthDay = model.BirthDay;
                    account.Country = model.Country;
                    account.Description = model.Description;
                    account.Thumb = model.Thumb;
                    _context.Update(account);
                    _context.SaveChanges();

                    return RedirectToAction("EditProfile", "AdminAccounts", new { Area = "Admin" });
                }
                catch
                {
                    return View(model);
                }
            }
            return View();

        }
    }
}
