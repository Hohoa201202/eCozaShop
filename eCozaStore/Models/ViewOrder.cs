using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("ViewOrder")]
    public class ViewOrder
    {
        // Bảng sản phẩm
        [Key]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public bool Active { get; set; } // Trạng thái sản phẩm
        public int? UnitsInStock { get; set; } // Tồn kho
        public int? Price { get; set; }

        // Bảng khách hàng
        public int? CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        // Bảng đơn hàng
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; } // Ngày đặt hàng
        public DateTime? PaymentDate { get; set; } // Ngày thanh toán
        public bool? Paid { get; set; } // Trả
        public bool? Deleted { get; set; } // đã xóa đơn hàng

        // Bảng chi tiết đơn hàng
        public int? Discount { get; set; } // giảm giá
        public int? Quantity { get; set; } // số lượng
        public int? Total => (Quantity * Price) - Discount;  // tổng tiền
        public DateTime? ShipDate { get; set; } // ngày giao hàng

        // Bảng trạng thái đơn hàng
        public int? TransactStatusId { get; set; }
        public string? Status { get; set; }
    }
}
