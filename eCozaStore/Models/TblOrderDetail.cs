using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("tblOrderDetails")]
    public partial class TblOrderDetail
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

        public  TblOrder? Order { get; set; }
        public  TblProduct? Product { get; set; }
    }
}
