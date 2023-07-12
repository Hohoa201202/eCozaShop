using eCozaStore.Models;
using eCozaStore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace eCozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminPostsController : Controller
    {
        private readonly dbCozaStoreContext _context;

        public AdminPostsController(dbCozaStoreContext context)
        {
            _context = context;
        }


        public IActionResult Index(int page = 1)
        {

            var pageNumber = page;
            var pageSize = Functions.PAGE_SIZE;

            var lsPosts = (from post in _context.TblPosts
                           orderby post.PostId descending
                           select post).ToPagedList(pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;

            // Lấy ra danh mục

            var CategoryList = (from cat in _context.TblCategories
                                select new SelectListItem()
                                {
                                    Text = cat.CategoryName,
                                    Value = cat.CategoryId.ToString(),
                                }).ToList();
            CategoryList.Insert(0, new SelectListItem()
            {
                Text = "----Chọn danh mục ----",
                Value = "0"
            });

            ViewBag.CategoryList = CategoryList;


            // 

            return View(lsPosts);
        }



        public IActionResult Details(int? postID)
        {
            if (postID == null || postID == 0)
            {
                return NotFound();
            }

            var tblPosts = _context.TblPosts
                           .Include(x => x.Category)
                           .FirstOrDefault(x => x.PostId == postID);

            if (tblPosts == null)
            {
                return NotFound();
            }

            return View(tblPosts);
        }


        // ============

        public IActionResult Create()
        {

            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(TblPost tblPost)
        {
            if (ModelState.IsValid)
            {


                tblPost.CreatedDate = DateTime.Now;
                tblPost.Status = 3;

                _context.Add(tblPost);
                _context.SaveChanges();



                return RedirectToAction("Index");
            }
            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblPost.CategoryId);

            return View(tblPost);
        }


        // ====================================

        public IActionResult Edit(int? postID)
        {
            if (postID == null || postID == 0)
            {
                return NotFound();
            }

            var tblPosts = _context.TblPosts.Find(postID);

            if (tblPosts == null)
            {
                return NotFound();

            }
            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblPosts.CategoryId);

            return View(tblPosts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TblPost tblPost)
        {
            if (ModelState.IsValid)
            {


                _context.Update(tblPost);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewData["DanhMuc"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblPost.CategoryId);
            return View(tblPost);
        }


        // ===================================
        public IActionResult Delete(int? postID)
        {
            if (postID == null || postID == 0)
            {
                return NotFound();
            }

            var tblPosts = _context.TblPosts
                            .Include(x => x.Category)
                            .FirstOrDefault(x => x.PostId == postID);

            if (tblPosts == null)
            {
                return NotFound();
            }

            return View(tblPosts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int postID)
        {
            var tblPosts = _context.TblPosts
                           .Where(x => x.PostId == postID)
                           .FirstOrDefault();

            if (tblPosts == null)
            {
                return NotFound();

            }

            _context.TblPosts.Remove(tblPosts);
            _context.SaveChanges();

            if (tblPosts.Thumb != null)
            {
                // Xóa ảnh 
                var fileName = "wwwroot" + tblPosts.Thumb;
                System.IO.File.Delete(fileName);
            }

            return RedirectToAction("Index");
        }


        //========================================================================================

        //GET: Duyệt bài viết
        public IActionResult IndexApprovePost(int page = 1)
        {
            var pageNumber = page;
            var pageSize = Functions.PAGE_SIZE;

            var lsPosts = (from post in _context.TblPosts
                        where (post.Status == 1)
                        join stt in _context.TblPostStatus on post.Status equals stt.Status
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

            ViewBag.CurrentPage = pageNumber;

            // Lấy ra danh mục
            var CategoryList = (from cat in _context.TblCategories
                                select new SelectListItem()
                                {
                                    Text = cat.CategoryName,
                                    Value = cat.CategoryId.ToString(),
                                }).ToList();
            CategoryList.Insert(0, new SelectListItem()
            {
                Text = "----Chọn danh mục ----",
                Value = "0"
            });

            ViewBag.CategoryList = CategoryList;

            return View(lsPosts);
        }

        //Phê duyệt bài viết
        public IActionResult ApprovePost(int? postID)
        {
            var tblPosts = _context.TblPosts
                           .Where(x => x.PostId == postID)
                           .FirstOrDefault();

            if (tblPosts == null)
            {
                return NotFound();
            }

            tblPosts.IsActive = true;
            tblPosts.Status = 3;

            _context.TblPosts.Update(tblPosts);
            _context.SaveChanges();

            return RedirectToAction("IndexApprovePost");
        }

        //Từ chối bài viết
        public IActionResult DeclinePost(int? postID)
        {
            var tblPosts = _context.TblPosts
                           .Where(x => x.PostId == postID)
                           .FirstOrDefault();

            if (tblPosts == null)
            {
                return NotFound();
            }

            tblPosts.Status = 2;

            _context.TblPosts.Update(tblPosts);
            _context.SaveChanges();

            return RedirectToAction("IndexApprovePost");
        }
    }
}
