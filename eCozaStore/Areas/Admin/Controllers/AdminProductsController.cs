using eCozaStore.Models;
using eCozaStore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using PagedList.Core;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly dbCozaStoreContext _context;

        public AdminProductsController(dbCozaStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int CategoryID = 0)
        {
            var pageNumber = page;
            var pageSize = Functions.PAGE_SIZE;

            List<TblProduct> lsProducts = new List<TblProduct>();

            if (CategoryID != 0)
            {
                lsProducts = (from p in _context.TblProducts
                              where p.CategoryId == CategoryID
                              join c in _context.TblCategories on p.CategoryId equals c.CategoryId
                              orderby p.ProductId descending
                              select p).ToList();
            }
            else
            {
                lsProducts = (from p in _context.TblProducts
                              join c in _context.TblCategories on p.CategoryId equals c.CategoryId
                              orderby p.ProductId descending
                              select p).ToList();
            }

            PagedList<TblProduct> models = new PagedList<TblProduct>(lsProducts.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentCategoryID = CategoryID;
            ViewBag.CurrentPage = pageNumber;
            


            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName");

          



            return View(models);
        }


        // PHẦN NÀY ĐỂ LỌC DANH MỤC
        public IActionResult Filtter(int CategoryID = 0)
        {
            var url = $"/Admin/AdminProducts?CategoryId={CategoryID}";
            if (CategoryID == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = "success", redirectUrl = url });
        }


        public IActionResult Details(int? productID)
        {
            if (productID == null || productID == 0)
            {
                return NotFound();
            }

            var tblProduct = _context.TblProducts
                .Include(x => x.Category)
                .FirstOrDefault(x => x.ProductId == productID);

            if (tblProduct == null)
            {
                return NotFound();
            }

            return View(tblProduct);
        }

        // ================================ === === Tạo mới sản phẩm
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblProduct tblProduct)
        {
            if (ModelState.IsValid)
            {
                
                
                tblProduct.ModifiedDate = DateTime.Now; 
                tblProduct.CreatedDate = DateTime.Now;
                _context.Add(tblProduct);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblProduct.CategoryId);
            return View(tblProduct);
        }





        // =========================== Chỉnh sửa sản phẩm

        public IActionResult Edit(int? productID)
        {
            if (productID == null || productID == 0)
            {
                return NotFound();
            }

            var tblProduct = _context.TblProducts.Find(productID);

            if (tblProduct == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblProduct.CategoryId);


            return View(tblProduct);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TblProduct tblProduct)
        {
            if (ModelState.IsValid)
            {

                
                tblProduct.ModifiedDate = DateTime.Now; // giá trị này tự tạo ra bởi hệ thông khi người dùng nhập vào

                _context.Update(tblProduct);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(tblProduct);
        }


        // ======================= Xóa sản phẩm

        public IActionResult Delete(int? productID)
        {
            if (productID == null || productID == 0)
            {
                return NotFound();
            }

            var tblProduct = _context.TblProducts
                            .Include(x => x.Category)
                            .FirstOrDefault(x => x.ProductId == productID);

            if (tblProduct == null)
            {
                return NotFound();

            }
            return View(tblProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int productID)
        {
            var tblProduct = _context.TblProducts
                            .Include(x => x.Category)
                            .FirstOrDefault(x => x.ProductId == productID);


            var lsImagesProduct = (from i in _context.TblImageProducts
                                   where i.ProductId == productID
                                   select i).ToList();
                                    
            if (tblProduct == null)
            {
                return NotFound();
            }

            var check = _context.TblOrderDetails
                        .FirstOrDefault(x => x.ProductId == productID);

            if (check == null)
            {
                for (int i = 1; i < lsImagesProduct.Count; i++)
                {
                    _context.TblImageProducts.Remove(lsImagesProduct[i]);
                    _context.SaveChanges();
                }

                _context.TblProducts.Remove(tblProduct);

                if (tblProduct.Thumb != null)
                {
                    // Xóa ảnh cũ
                    var fileName = "wwwroot" + tblProduct.Thumb;
                    System.IO.File.Delete(fileName);
                }
            }
            else
            {
                tblProduct.Active = false;
                _context.TblProducts.Update(tblProduct);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }







        // =========================== Hàm upload hình ảnh chi tiết sản phẩm
        [HttpGet]
        public IActionResult UploadImages(int productID)
        {
            var tblProduct = _context.TblProducts
                                     .Where(p => p.ProductId == productID)
                                     .Include(p => p.ImageProduct)
                                     .FirstOrDefault();

            if (tblProduct == null)
            {
                return NotFound("Không có sản phẩm");
            }


            ViewData["SanPham"] = tblProduct;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages(int id, IFormFile? fThumb)
        {

            var tblProduct = _context.TblProducts
                                    .Where(p => p.ProductId == id)
                                    .Include(p => p.ImageProduct)
                                    .FirstOrDefault();

            if (tblProduct == null)
            {
                return NotFound("Không có sản phẩm");
            }

            if (ModelState.IsValid)
            {


                if (fThumb != null) // nếu có chọn hình ảnh
                {

                    var file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                                    + Path.GetExtension(fThumb.FileName);

                    var file = Path.Combine("wwwroot", "files", "images", "products", file1);

                    using (var filestream = new FileStream(file, FileMode.Create))
                    {
                        await fThumb.CopyToAsync(filestream);
                    }


                    _context.Add(new TblImageProduct()
                    {
                        ProductId = tblProduct.ProductId,
                        ImagePath = file1,
                        IsActive = true,
                        IsDefault = false,
                    });
                    _context.SaveChanges();
                }


                return RedirectToAction("Index");
            }

            ViewData["SanPham"] = tblProduct;

            return View();
        }

        // Upload file 
        [HttpPost]
        public async Task<IActionResult> UploadImageAPI(int id, IFormFile? fThumb)
        {

            var tblProduct = _context.TblProducts
                                    .Where(p => p.ProductId == id)
                                    .Include(p => p.ImageProduct)
                                    .FirstOrDefault();

            if (tblProduct == null)
            {
                return NotFound("Không có sản phẩm");
            }

            if (ModelState.IsValid)
            {


                if (fThumb != null) // nếu có chọn hình ảnh
                {

                    var file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                                    + Path.GetExtension(fThumb.FileName);

                    var file = Path.Combine("wwwroot", "files", "images", "products", file1);

                    using (var filestream = new FileStream(file, FileMode.Create))
                    {
                        await fThumb.CopyToAsync(filestream);
                    }


                    _context.Add(new TblImageProduct()
                    {
                        ProductId = tblProduct.ProductId,
                        ImagePath = file1,
                        IsActive = true,
                        IsDefault = false,
                    });
                    _context.SaveChanges();
                }


            }



            return Ok();
        }


        [HttpPost]
        public IActionResult ListImages(int id)
        {
            var product = _context.TblProducts
                                   .Where(p => p.ProductId == id)
                                   .Include(p => p.ImageProduct)
                                   .FirstOrDefault();

            if (product == null)
            {
                return Json(
                    new
                    {
                        success = 0,
                        message = "san pham ko tim thay",
                    }

                );
            }


            var listimage = product.ImageProduct.Select(image => new
            {
                id = image.ImageId,
                imagePath = "/files/images/products/" + image.ImagePath,
            });

            return Json(
                new
                {
                    success = 1,
                    images = listimage,
                }
            );
        }



        // Xóa ảnh


        [HttpPost]
        public IActionResult DeleteImages(int id)
        {
            var tblImagesProduct = _context.TblImageProducts
                          .Where(x => x.ImageId == id)
                          .FirstOrDefault();


            if (tblImagesProduct != null)
            {

                _context.Remove(tblImagesProduct);
                _context.SaveChanges();

                var filename = "wwwroot/files/images/products/" + tblImagesProduct.ImagePath;
                System.IO.File.Delete(filename);
            }
            return Ok();
        }
    }
}