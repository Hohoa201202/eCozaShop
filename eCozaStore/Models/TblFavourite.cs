using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace eCozaStore.Models
{
    [Table("tblFavourites")]
    public partial class TblFavourite
    {
        [Key]
        public int FavouriteId { get; set; }
	    public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
