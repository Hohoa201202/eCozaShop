using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("tblOrders")]
    public partial class TblOrder
    {

        [Key]
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? TransactStatusId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public bool? Deleted { get; set; }
        public bool? Paid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool? PaymentId { get; set; }
        public string? Description { get; set; }

        public  TblCustomer? Customer { get; set; }
        public  TblTransactStatus? TransactStatus { get; set; }
      
    }
}
