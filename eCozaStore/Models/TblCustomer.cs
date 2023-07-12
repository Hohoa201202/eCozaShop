using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("tblCustomers")]
    public partial class TblCustomer
    {
     

        [Key]
        public int CustomerId { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool Active { get; set; }
        public string? Description { get; set; }

       
    }
}
