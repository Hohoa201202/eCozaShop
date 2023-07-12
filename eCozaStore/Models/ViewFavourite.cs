using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace eCozaStore.Models
{
    [Table("ViewFavourite")]
    public partial class ViewFavourite
    {
        [Key]
        public int FavouriteId { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? ProductName { get; set; } 
        public string? Thumb { get; set; } 
        public int? Discount { get; set; } 
        public int? Price { get; set; }
    }
}
