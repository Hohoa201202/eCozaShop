using System.Threading.Tasks;

namespace eCozaStore.Models
{
    //Session
    public class CartItem
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int? Price { get; set; }
        public string? Thumb { get; set; }
        public int? Quantity { get; set; }
        public int? Total => Quantity * Price;
    }
}
