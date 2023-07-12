using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("tblLocations")]
    public partial class TblLocation
    {
        [Key]
        public int LocationId { get; set; }
        public int? CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsDefault { get; set; }
        public string? Description { get; set; }
    }
}
