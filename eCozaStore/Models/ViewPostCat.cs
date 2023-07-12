using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("View_Post_Cat")]
    public partial class ViewPostCat
    {
        [Key]
        public int PostId { get; set; }
        public int? CategoryId { get; set; }
        public string? Title { get; set; }
        public string? Abstract { get; set; }
        public string? Contents { get; set; }
        public string? Thumb { get; set; }
        public bool? Published { get; set; }
        public bool IsActive { get; set; }
        public int? PostOrder { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Author { get; set; }
        public string? Tags { get; set; }
        public bool IsHot { get; set; }
        public bool IsNewfeed { get; set; }
        public string? Alias { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }
        public int? Sview { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
    }
}
