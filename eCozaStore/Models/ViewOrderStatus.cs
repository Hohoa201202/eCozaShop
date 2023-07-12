using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    public class ViewOrderStatus
    {
        // Bảng khách hàng
        public int? CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        // Bảng đơn hàng
        [Key]
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; } // Ngày đặt hàng


        // Bảng trạng thái đơn hàng
        public int? TransactStatusId { get; set; }
        public string? Status { get; set; }
    }
}
