using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("tblImageProducts")]
    public partial class TblImageProduct
    {
        [Key]
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public string? ImagePath { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        [ForeignKey("ProductId")]
        public TblProduct Product { get; set; }
    }
}
