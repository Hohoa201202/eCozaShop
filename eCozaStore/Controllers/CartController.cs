using eCozaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using PagedList.Core;
using Newtonsoft.Json;
using System.IO.Pipelines;

namespace eCozaStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly dbCozaStoreContext _dbCozaStoreContext;
        public static int notify = 0; //Biến hiển thị có bao nhiêu sản phẩm trong giỏ hàng

        public CartController(ILogger<CartController> logger, dbCozaStoreContext conText)
        {
            _logger = logger;
            _dbCozaStoreContext = conText;
        }

        [Route("/addcart", Name = "addcart")]
        public IActionResult AddToCart(CartItem model)
        {

            var product = _dbCozaStoreContext.TblProducts
                            .Where(p => p.ProductId == model.ProductID)
                            .FirstOrDefault();
            if (product == null)
                return NotFound();

            // Xử lý đưa vào Cart ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.ProductID == model.ProductID);

            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.Quantity += model.Quantity;
            }
            else
            {
                //  Thêm mới
                model.ProductName = product.ProductName;
                model.Price = product.Price;
                model.Thumb = product.Thumb;
                cart.Add(model);

                //Tăng lên 1
                notify++;
            }

            // Lưu cart vào Session
            SaveCartSession(cart);

            // Chuyển đến trang hiện thị Cart
            return RedirectToAction(nameof(Cart));
        }

        // Xóa item trong giỏ hàng
        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int productid)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.ProductID == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, xóa cartitem khỏi ds cart
                cart.Remove(cartitem);
            }

            notify--;

            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        //  Cập nhật giỏ hàng
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int productid, [FromForm] int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.ProductID == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.Quantity = quantity;
            }

            if(quantity == 0)
            {
                string returnUrl = "/removecart/" + productid.ToString();
                return Redirect(returnUrl);
            }

            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }


        // Hiện thị giỏ hàng
        [Route("gio-hang.html", Name = "ViewCart")]
        public IActionResult Cart()
        {
            return View(GetCartItems());
        }

        [Route("/checkout")]
        public IActionResult CheckOut()
        {

            var AccountID = HttpContext.Session.GetString("AccID");
            if (AccountID == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // Lấy list các CartItem từ Session
            var listCart = GetCartItems();

            //Gán dữ liệu cho TblOrder
            TblOrder objTblOrder = new TblOrder();
            objTblOrder.OrderDate = DateTime.Now;
            objTblOrder.CustomerId = int.Parse(AccountID);
            objTblOrder.TransactStatusId = 1;

            //Lưu dữ liệu vào TblOrder
            _dbCozaStoreContext.TblOrders.Add(objTblOrder);
            _dbCozaStoreContext.SaveChanges();

            //Lấy OrderID vừa tạo
            int _OrderID = objTblOrder.OrderId;

            List<TblOrderDetail> listTblOrderDetails = new List<TblOrderDetail>();

            foreach (var item in listCart)
            {
                TblOrderDetail obj = new TblOrderDetail();
                obj.OrderId = _OrderID;
                obj.ProductId = item.ProductID;
                obj.Quantity = item.Quantity;
                obj.Total = item.Total;

                listTblOrderDetails.Add(obj);
            }

            _dbCozaStoreContext.TblOrderDetails.AddRange(listTblOrderDetails);
            _dbCozaStoreContext.SaveChanges();

            notify = 0;

            ClearCart();
            return RedirectToAction("Order", "Cart");
        }


        // Xem đơn hàng
        [Route("quan-ly-don-hang.html", Name = "ViewOrder")]
        public IActionResult Order(int page = 1)
        {
            string AccountID = HttpContext.Session.GetString("AccID");

            var pageNumber = page;
            var pageSize = 3;

            if (AccountID == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var list = (from c in _dbCozaStoreContext.TblCustomers
                        join o in _dbCozaStoreContext.TblOrders on c.CustomerId equals o.CustomerId
                        join s in _dbCozaStoreContext.TblTransactStatuses on  o.TransactStatusId equals s.TransactStatusId
                        where (c.CustomerId == int.Parse(AccountID))
                        orderby o.OrderDate descending
                        select new ViewOrderStatus
                        {
                            CustomerId = c.CustomerId,
                            OrderDate = o.OrderDate,
                            FullName = c.FullName,
                            Phone = c.Phone,
                            Address = c.Address,
                            OrderId = o.OrderId,
                            Status= s.Status
                        }).ToPagedList(pageNumber, pageSize);

            return View(list);
        }


        // Session
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
        List<CartItem> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }
    }
}
