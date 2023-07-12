using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("tblCategories")]
    public partial class TblCategory
    {
      

        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục!")]
        public string? CategoryName { get; set; }
        public string? ShortName { get; set; }
        public int? ParentId { get; set; }
        public int? Levels { get; set; }
        public bool IsActive { get; set; }
        public int? CategoryOrder { get; set; }
        public bool? Published { get; set; }
        public string? Thumb { get; set; }
        public bool HomeFlag { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }
        public string? Cover { get; set; }
        public string? SchemaMarkup { get; set; }
        public string? Description { get; set; }

      
    }
}
