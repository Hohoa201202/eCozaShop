using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("tblProducts")]
    public partial class TblProduct
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm!")]
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

        public TblOrderDetail? OrderDetails { get; set; }
        public TblCategory? Category { get; set; }

        public List<TblImageProduct>? ImageProduct { get; set; }
    }
}
