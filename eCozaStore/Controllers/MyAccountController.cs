using eCozaStore.Models;
using eCozaStore.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PagedList.Core;
using System.Security.Claims;

namespace eCozaStore.Controllers
{
    public class MyAccountController : Controller
    {

        private readonly ILogger<MyAccountController> _logger;
        private readonly dbCozaStoreContext _dbContext;

        public MyAccountController(ILogger<MyAccountController> logger, dbCozaStoreContext dbCozaStoreContext)
        {
            _logger = logger;
            _dbContext = dbCozaStoreContext;
        }

        // GET: MyAccountController
        [Route("ho-so-ca-nhan.html", Name = "MyAcc")]
        public ActionResult Index()
        {
            var CustomerId = HttpContext.Session.GetString("AccID");

            if (CustomerId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var list = _dbContext.TblCustomers.Find(int.Parse(CustomerId));

            if (list == null)
            {
                return NotFound();
            }
            return View(list);
        }

        //POST: MyAccountController
        [Route("ho-so-ca-nhan.html", Name = "MyAcc")]
        [HttpPost]
        public async Task<ActionResult> Index(TblCustomer model, IFormFile? fThumb)
        {
            //Kiểm tra số điện thoại có bị trùng ?
            var listCheck = (from l in _dbContext.TblCustomers
                             where (l.Phone == model.Phone) && (l.CustomerId != model.CustomerId)
                             select l).ToList();

            if (listCheck.Count == 0) //SĐT không bị trùng
            {
                if (ModelState.IsValid)
                {
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Functions.SEOUrl(model.FullName + model.CustomerId.ToString()) + extension;
                        model.Avatar = await Functions.UploadFile(fThumb, @"account", image.ToLower());
                    }
                    else
                    {
                        //...
                    }

                    if (string.IsNullOrEmpty(model.Avatar))
                    {
                        model.Avatar = "default.jpg";
                    }

                    _dbContext.Update(model);
                    _dbContext.SaveChanges();

                    var data = _dbContext.TblCustomers.Find(model.CustomerId);

                    HttpContext.SignOutAsync();
                    //Identity
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, data.FullName),
                        new Claim("Email", data.Email),
                        new Claim("Phone", data.Phone),
                        new Claim("ID", data.CustomerId.ToString()),
                        new Claim("Avt", data.Avatar)
                    };

                    var gran = new ClaimsIdentity(userClaims, "User");
                    var userPr = new ClaimsPrincipal(new[] { gran });
                    await HttpContext.SignInAsync(userPr);

                    ViewBag.Err = "Cập nhật thông tin thành công";

                    return View(model);
                    //return Redirect("ho-so-ca-nhan.html");
                }
            }
            else
            {
                ViewBag.Err = "Số điện thoại đã tồn tại trong hệ thống";
            }

            var list = _dbContext.TblCustomers.Find(model.CustomerId);

            return View(list);
        }

        //GET: ĐỔi mật khẩu
        [Route("doi-mat-khauu.html", Name = "ChangePass")]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            var AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID == null)
            {
                //Remove session
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccID");
            }
            return View();
        }

        //POST: Đổi mật khẩu)
        [Route("doi-mat-khauu.html", Name = "ChangePass")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword model)
        {
            string AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID == null)
            {
                //Remove session
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccID");
            }

            if (ModelState.IsValid)
            {
                var acc = _dbContext.TblCustomers.AsNoTracking().FirstOrDefault(a => a.CustomerId == int.Parse(AccountID));

                if (acc == null)
                {
                    return NotFound();
                }

                try
                {
                    //Lấy pass cũ để kiểm tra
                    string passNow = Functions.GetMD5(model.PasswordNow.Trim());

                    if (passNow == acc.Password.Trim()) //Đúng pass
                    {
                        //Tạo pass mới
                        acc.Password = Functions.GetMD5(model.Password);
                        _dbContext.TblCustomers.Update(acc);
                        _dbContext.SaveChanges();

                        ViewBag.Error = "Đổi mật khẩu thành công";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Mật khẩu cũ không đúng";
                        return View();
                    }
                }
                catch
                {
                    return RedirectToAction("Index", "MyAccount");
                }
            }
            return View();
        }

        //GET: Quên mật khẩu Forgotpassword
        [Route("quen-mat-khauu.html", Name = "ForgotPass")]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            string AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //POST: Quên mật khẩu Forgotpassword
        [Route("quen-mat-khauu.html", Name = "ForgotPass")]
        [HttpPost]
        public ActionResult ForgotPassword(TblCustomer model, string NewPass, string NewPass2)
        {
            string AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID != null)
            {
                return RedirectToAction("Index", "Home");
            }

            var cus = _dbContext.TblCustomers.FirstOrDefault(item => (item.Phone == model.Phone) && (item.Email == model.Email));

            if (cus == null)
            {
                ViewBag.Error = "Số điện thoại hoặc Email không chính xác !";
                return View();
            }
            if (cus.Active == false)
            {
                ViewBag.Error = "Tài khoản đã bị vô hiệu hóa !";
                return View();
            }
            if (NewPass.Trim() != NewPass2.Trim())
            {
                ViewBag.Error = "Mật khẩu không khớp nhau !";
                return View();
            }

            cus.Password = Functions.GetMD5(NewPass);

            _dbContext.TblCustomers.Update(cus);
            _dbContext.SaveChanges();

            ViewBag.Error = "Thành công";

            return View();
        }

        //GET: Xem sản phẩm yêu thích
        [Route("san-pham-yeu-thich.html", Name = "Favourite")]
        public ActionResult Favourite(int page = 1)
        {
            var pageNumber = 10;
            var pageSize = page;

            var AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var model = (from f in _dbContext.ViewFavourites
                         where f.CustomerID == int.Parse(AccountID)
                         orderby f.CreatedDate descending
                         select f).ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        // Add sản phẩm yêu thích
        [Route("them-san-pham-yeu-thich.html", Name = "AddFavourite")]
        public ActionResult AddFavourite(int ProductID)
        {
            var AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TblFavourite obj = new TblFavourite();

            obj.CustomerID = int.Parse(AccountID);
            obj.ProductID = ProductID;
            obj.CreatedDate = DateTime.Now;

            _dbContext.TblFavourites.Add(obj);
            _dbContext.SaveChanges();

            return RedirectToAction("Favourite", "MyAccount");
        }

        // Quản lý địa chỉ
        [Route("quan-ly-dia-chi.html", Name = "MyAddress")]
        public ActionResult MyAddress()
        {
            var AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var model = (from item in _dbContext.TblLocations
                         where item.CustomerId == int.Parse(AccountID)
                         select item).ToList();

            return View(model);
        }

        //Thêm địa chỉ
        [Route("them-dia-chi.html", Name = "CreateAddress")]
        [HttpPost]
        public ActionResult CreateAddress(TblLocation model)
        {
            var AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID == null)
            {
                return RedirectToAction("Login", "Home");
            }

            model.CustomerId = int.Parse(AccountID);
            model.IsDefault = false;

            _dbContext.TblLocations.Add(model);
            _dbContext.SaveChanges();

            return RedirectToAction("MyAddress", "MyAccount");
        }

        //Sửa địa chỉ
        [Route("sua-dia-chi.html", Name = "EditAddress")]
        [HttpPost]
        public ActionResult EditAddress(TblLocation model)
        {
            var AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID == null)
            {
                return RedirectToAction("Login", "Home");
            }

            _dbContext.TblLocations.Update(model);
            _dbContext.SaveChanges();

            return RedirectToAction("MyAddress", "MyAccount");
        }

        //Xóa địa chỉ
        [Route("/deladdress", Name = "deladdress")]
        public IActionResult DelAddress(int locationid)
        {
            var location = _dbContext.TblLocations
                            .FirstOrDefault(l => l.LocationId == locationid);

            if (location == null)
            {
                return NotFound();
            }

            _dbContext.TblLocations.Remove(location);
            _dbContext.SaveChanges();

            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        //Đăng bài viết
        [Route("dang-bai.html", Name = "Post")]
        [HttpPost]
        public async Task<IActionResult> Post(TblPost model, IFormFile? fThumb)
        {
            var AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var cus = _dbContext.TblCustomers.Find(int.Parse(AccountID));

            if (ModelState.IsValid)
            {
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Functions.SEOUrl(model.Title) + extension;
                    model.Thumb = "/files/images/posts/" + await Functions.UploadFile(fThumb, @"posts", image.ToLower());
                }
                else
                {
                    //...
                }

                if (string.IsNullOrEmpty(model.Thumb))
                {
                    model.Thumb = "default.jpg";
                }

                model.CreatedDate = DateTime.Now;
                model.Author = cus.FullName;
                model.Tags = AccountID.Trim();
                model.Published = true;
                model.IsActive = false;
                model.PostOrder = 1;
                model.Status = 1;
                model.IsHot = false;
                model.IsNewfeed = false;
                model.Sview = 1;
                if(model.CategoryId == null)
                {
                    model.CategoryId = 3;
                }

                _dbContext.TblPosts.Add(model);
                _dbContext.SaveChanges();

                return RedirectToAction("MyPost", "MyAccount");
            }

            return RedirectToAction("Blog", "Home");
        }

        //Quản lý bài viết
        [Route("quan-ly-bai-viet.html", Name = "MyPost")]
        public IActionResult MyPost(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            var AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var list = (from post in _dbContext.TblPosts
                        where (post.Tags.Trim() == AccountID.Trim())
                        join stt in _dbContext.TblPostStatus on post.Status equals stt.Status
                        orderby post.CreatedDate descending
                        select new ViewPostStatus
                        {
                            StatusName = stt.StatusName,
                            PostId = post.PostId,
                            Title = post.Title,
                            Abstract = post.Abstract,
                            CreatedDate = post.CreatedDate,
                            CategoryId = post.CategoryId,
                            Contents = post.Contents,
                            Thumb = post.Thumb,
                            Published = post.Published,
                            IsActive = post.IsActive,
                            PostOrder = post.PostOrder,
                            Status = post.Status,
                            Author = post.Author,
                            Tags = post.Tags,
                            IsHot = post.IsHot,
                            IsNewfeed = post.IsNewfeed,
                            Sview = post.Sview,

                        }).ToPagedList(pageNumber, pageSize);

            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        //Xóa bài viết
        [Route("xoa-bai-viet.html", Name = "DelMyPost")]
        public IActionResult DelMyPost(int postid, int page = 1)
        {
            var post = _dbContext.TblPosts.FirstOrDefault(l => l.PostId == postid);

            if (post == null)
            {
                return NotFound();
            }

            _dbContext.TblPosts.Remove(post);
            _dbContext.SaveChanges();

            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        //Sửa bài viết
        [Route("chinh-sua-bai-viet-{id:int}.html", Name = "EditMyPost")]
        public IActionResult EditMyPost(int? id)
        {
            var post = _dbContext.TblPosts.Find(id);

            var catList = (from m in _dbContext.TblCategories
                           select new SelectListItem()
                           {
                               Text = m.CategoryName,
                               Value = m.CategoryId.ToString(),
                           }).ToList();

            catList.Insert(0, new SelectListItem()
            {
                Text = "--- Chọn danh mục ---",
                Value = String.Empty
            });

            ViewBag.catList = catList;

            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return View(post);
        }

        //POST: Sửa bài viết
        [Route("chinh-sua-bai-viet-{id:int}.html", Name = "EditMyPost")]
        [HttpPost]
        public IActionResult EditMyPost(TblPost model)
        {
            model.Status = 1;
            _dbContext.TblPosts.Update(model);
            _dbContext.SaveChanges();

            return RedirectToAction("MyPost", "MyAccount");
        }
    }
}
