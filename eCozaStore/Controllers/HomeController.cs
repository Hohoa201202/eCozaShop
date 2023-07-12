using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using PagedList.Core;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using eCozaStore.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCozaStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbCozaStoreContext _dbCozaStoreContext;

        public HomeController(ILogger<HomeController> logger, dbCozaStoreContext dbCozaStoreContext)
        {
            _logger = logger;
            _dbCozaStoreContext = dbCozaStoreContext;
        }

        public async Task<IActionResult> Index()
        {
            var AccountID = HttpContext.Session.GetString("AccountId");
            if (AccountID != null)
            {
                await HttpContext.SignOutAsync();

                var AccID = HttpContext.Session.GetString("AccID");

                if (AccID != null)
                {
                    var data = _dbCozaStoreContext.TblCustomers.Find(int.Parse(AccID));
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
                }
            }
            return View();
        }


        //GET: Đăng ký
        [Route("register.html", Name = "Register")]
        [HttpGet]
        public ActionResult Register()
        {
            var AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        //POST: Đăng ký
        [Route("register.html", Name = "Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(TblCustomer _Customer)
        {
            if (ModelState.IsValid)
            {
                var check = _dbCozaStoreContext.TblCustomers.FirstOrDefault(s => s.Phone == _Customer.Phone);
                if (check == null)
                {
                    _Customer.Password = Functions.GetMD5(_Customer.Password);
                    _Customer.Active = true;
                    _Customer.CreatedDate = DateTime.Now;
                    _Customer.Birthday = DateTime.Now;
                    _Customer.Address = "Việt Nam";

                    if (_Customer.Avatar == null)
                    {
                        _Customer.Avatar = "default.jpg";
                    }
                    if (_Customer.Email == null)
                    {
                        _Customer.Email = "Ecoza.vn@gmail.com";
                    }
                    if (_Customer.FullName == null)
                    {
                        _Customer.FullName = "No-Name";
                    }

                    _dbCozaStoreContext.TblCustomers.Add(_Customer);
                    _dbCozaStoreContext.SaveChanges();

                    //Lấy CustomerID vừa tạo
                    int _CustomerID = _Customer.CustomerId;

                    TblLocation _location = new TblLocation();
                    _location.CustomerId = _CustomerID;
                    _location.FullName = _Customer.FullName;
                    _location.Phone = _Customer.Phone;
                    _location.Address = _Customer.Address;
                    _location.IsDefault = true;

                    _dbCozaStoreContext.TblLocations.Add(_location);
                    _dbCozaStoreContext.SaveChanges();

                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ViewBag.error = "Số điện thoại đã tồn tại !";
                    return View();
                }
            }
            return View();
        }


        //Get: Đăng nhập
        [Route("/login.html", Name = "Login")]
        [HttpGet]
        public ActionResult Login()
        {
            var AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }


        //Post: Đăng nhập
        [Route("/login.html", Name = "Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountLogin model, string? returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var f_password = Functions.GetMD5(model.Password.Trim());

                    var data = _dbCozaStoreContext.TblCustomers.SingleOrDefault(p => p.Phone.ToLower() == model.Phone.ToLower().Trim());

                    if (data == null)
                    {
                        ViewBag.Error = "Thông tin đăng nhập không chính xác";
                        return View(model);
                    }

                    if (data.Password.Trim() != f_password) //Sửa đây
                    {
                        ViewBag.Error = "Thông tin đăng nhập không chính xác";
                        return View(model);
                    }

                    if (data.Active == false)
                    {
                        ViewBag.Error = "Tài khoản đã bị khóa !";
                        return View(model);
                    }

                    data.LastLogin = DateTime.Now;
                    _dbCozaStoreContext.TblCustomers.Update(data);
                    _dbCozaStoreContext.SaveChanges();

                    //Add session
                    HttpContext.Session.SetString("AccID", data.CustomerId.ToString());

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

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Home");
            }

            return RedirectToAction("Login", "Home");
        }

        //Đăng xuất
        public ActionResult Logout()
        {
            //Remove session
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("AccID");

            return RedirectToAction("Login", "Home");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Danh sách bài viết
        [Route("/pages/[action].html", Name = "blog")]
        public IActionResult Blog(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 6;

            var lsPosts = (from post in _dbCozaStoreContext.ViewPostCats
                           where post.IsActive == true && post.Status == 3
                           orderby post.Sview descending
                           select post).ToPagedList(pageNumber, pageSize);

            var AccountID = HttpContext.Session.GetString("AccID");

            if (AccountID != null)
            {
                ViewBag.IsLogin = true;
            }

            var catList = (   from m in _dbCozaStoreContext.TblCategories
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

            return View(lsPosts);
        }

        //Chi tiết bài viết   
        [Route("/blogDetail/{slug}-{id:long}.html", Name = "BlogDetail")]
        [HttpGet]
        public IActionResult BlogDetail(long? id, int page = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _dbCozaStoreContext.TblPosts.FirstOrDefault(m => (m.PostId == id) && (m.IsActive == true));

            if (post.Sview != null)
            {
                post.Sview++;
            }
            else
            {
                post.Sview = 0;
            }

            _dbCozaStoreContext.Update(post);
            _dbCozaStoreContext.SaveChanges();

            if (post == null)
            {
                return NotFound();
            }

            var CustomerId = HttpContext.Session.GetString("AccID");

            if (CustomerId != null)
            {
                var cus = _dbCozaStoreContext.TblCustomers.Find(int.Parse(CustomerId));

                ViewBag.IsLogin = true;
                ViewBag.Name = cus.FullName;
                ViewBag.Avt = cus.Avatar;
            }
            else
            {
                ViewBag.IsLogin = false;
            }

            ViewBag.page = page;

            return View(post);
        }


        //Gửi bình luận
        //POST:
        [Route("/blogDetail/{slug}-{id:long}.html", Name = "BlogDetail")]
        [HttpPost]
        public IActionResult BlogDetail(TblComment _comment)
        {
            var CustomerId = HttpContext.Session.GetString("AccID");

            if (CustomerId == null)
            {
                if (_comment.Author == null)
                {
                    _comment.Author = "No-Name";
                }

                _comment.CreatedDate = DateTime.Now;
                _comment.IsActive = true;
                _comment.ParentId = 0;
                _comment.Slike = 0;
                _comment.Thumb = "default.jpg";
                _comment.CustomerId = 0;

                if (ModelState.IsValid)
                {
                    _dbCozaStoreContext.TblComments.Add(_comment);
                    _dbCozaStoreContext.SaveChanges();
                }
                return RedirectToAction("BlogDetail");
            }
            else
            {
                var Cus = _dbCozaStoreContext.TblCustomers.Find(int.Parse(CustomerId));

                if (Cus == null)
                {
                    return NotFound();
                }
                _comment.CustomerId = int.Parse(CustomerId);
                _comment.CreatedDate = DateTime.Now;
                _comment.IsActive = true;
                _comment.ParentId = 0;
                _comment.Slike = 0;
                _comment.Thumb = Cus.Avatar;
                _comment.Author = Cus.FullName;

                if (ModelState.IsValid)
                {
                    _dbCozaStoreContext.TblComments.Add(_comment);
                    _dbCozaStoreContext.SaveChanges();
                }
                return RedirectToAction("BlogDetail");
            }

            return View();
        }

        //Rep bình luận
        //POST:
        [HttpPost]
        public IActionResult RepComment(TblComment _comment)
        {
            var CustomerId = HttpContext.Session.GetString("AccID");
            var post = _dbCozaStoreContext.TblPosts.Find(_comment.PostId);

            string returnUrl = Functions.TitleSlugGeneration("/blogDetail", post.Title, post.PostId);

            if (CustomerId == null)
            {
                if (_comment.Author == null)
                {
                    _comment.Author = "No-Name";
                }

                _comment.CreatedDate = DateTime.Now;
                _comment.IsActive = true;
                if (_comment.ParentId == null)
                {
                    _comment.ParentId = 0;
                }
                _comment.Slike = 0;
                _comment.Thumb = "default.jpg";
                _comment.CustomerId = 0;

                if (ModelState.IsValid)
                {
                    _dbCozaStoreContext.TblComments.Add(_comment);
                    _dbCozaStoreContext.SaveChanges();
                }

                return Redirect(returnUrl);
            }
            else
            {
                var Cus = _dbCozaStoreContext.TblCustomers.Find(int.Parse(CustomerId));

                _comment.CustomerId = int.Parse(CustomerId);
                _comment.CreatedDate = DateTime.Now;
                _comment.IsActive = true;
                if (_comment.ParentId == null)
                {
                    _comment.ParentId = 0;
                }
                _comment.Slike = 0;
                _comment.Thumb = Cus.Avatar;
                _comment.Author = Cus.FullName;

                if (ModelState.IsValid)
                {
                    _dbCozaStoreContext.TblComments.Add(_comment);
                    _dbCozaStoreContext.SaveChanges();
                }

                return Redirect(returnUrl);
            }

            return Redirect(returnUrl);
        }

        //Trang giới thiệu
        [Route("/pages/[action].html", Name = "about")]
        public IActionResult About()
        {
            return View();
        }

        //Danh sách sản phẩm theo danh mục
        [Route("/list/{slug}-{CategoryId:int}.html", Name = "product")]
        public IActionResult Product(int? CategoryId, int page = 1)
        {

            var pageNumber = page;
            var pageSize = 16;

            var lsProducts = (from p in _dbCozaStoreContext.TblProducts
                              where (p.CategoryId == CategoryId) && (p.Active == true)
                              orderby p.Discount
                              select p).ToPagedList(pageNumber, pageSize);

            return View(lsProducts);
        }


        //Chi tiết sản phẩm
        [Route("/product/{slug}-{id:int}.html", Name = "productDetail")]
        [HttpGet]
        public IActionResult ProductDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lsProduct = (from p in _dbCozaStoreContext.TblProducts
                             join c in _dbCozaStoreContext.TblImageProducts on p.ProductId equals c.ProductId into t
                             from c in t.DefaultIfEmpty()
                             where (p.Active == true) && (p.ProductId == id)
                             orderby p.CreatedDate descending
                             select new ViewImageProduct
                             {
                                 ImagePath = c.ImagePath,
                                 ProductName = p.ProductName,
                                 Price = p.Price,
                                 Title = p.Title,
                                 ProductId = p.ProductId,
                                 CategoryId = p.CategoryId,
                                 Thumb = p.Thumb
                             }).ToList();

            if (lsProduct == null)
            {
                return NotFound();
            }

            var CustomerId = HttpContext.Session.GetString("AccID");

            if (CustomerId != null)
            {
                var cus = _dbCozaStoreContext.TblCustomers.Find(int.Parse(CustomerId));

                ViewBag.IsLogin = true;
                ViewBag.Name = cus.FullName;
                ViewBag.Avt = cus.Avatar;
            }

            return View(lsProduct);
        }


        //Đánh giá sản phẩm
        //POST:
        [Route("/product/{slug}-{id:int}.html", Name = "productDetail")]
        [HttpPost]
        public IActionResult ProductDetail(TblFeedback feedBack)
        {
            var CustomerId = HttpContext.Session.GetString("AccID");

            if (CustomerId == null)
            {
                return RedirectToAction("Login", "Home", new { });
            }

            if (ModelState.IsValid)
            {
                feedBack.CustomerId = int.Parse(CustomerId);
                feedBack.CreatedDate = DateTime.Now;
                feedBack.Status = true;
                feedBack.IsActive = true;
                feedBack.Slike = 0;
                _dbCozaStoreContext.TblFeedbacks.Add(feedBack);
                _dbCozaStoreContext.SaveChanges();

                return RedirectToAction("ProductDetail");
            }
            return RedirectToAction("ProductDetail");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}