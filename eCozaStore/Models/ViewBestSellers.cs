using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("ViewBestSellers")]
    public class ViewBestSellers
    {
        [Key]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ShortDesc { get; set; }
        public int? CategoryId { get; set; }
        public int? Price { get; set; }
        public int? Discount { get; set; }
        public string? Thumb { get; set; }
        public string? Video { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool BestSellers { get; set; }
        public bool HomeFlag { get; set; }
        public bool Active { get; set; }
        public string? Tags { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }
        public int? UnitsInStock { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }

        public int? Total => (Quantity * Price) - Discount; // tổng tiền
        public int? Sold => (Quantity - UnitsInStock); // đã bán
    }
}
