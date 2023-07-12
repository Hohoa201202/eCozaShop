using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    //[Table("ViewComments")]
    public partial class ViewComment
    {
        [Key]
        public int CommentId { get; set; }
        public int? PostId { get; set; }
        public int? CustomerId { get; set; }
        public int? ParentId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public string? Thumb { get; set; }
        public string? Author { get; set; }
        public int? Slike { get; set; }

        public string? FullName { get; set; }
        public string? Avatar { get; set; }
    }
}
