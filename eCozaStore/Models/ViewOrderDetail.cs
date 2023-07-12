using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
	[Table("ViewOrderDetails")]
	public class ViewOrderDetail
	{
        [Key]
        public int OrderDetailsId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? OrderNumber { get; set; }
        public int? Discount { get; set; }
        public int? Quantity { get; set; }
        public int? Total { get; set; }
        public DateTime? ShipDate { get; set; }
        public string? Description { get; set; }
        public int? CustomerId { get; set; }
        public string? Thumb { get; set; }
        public string? ProductName { get; set; }
        public int? Price { get; set; }

    }
}
