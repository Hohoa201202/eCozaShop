using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("ViewFeedback")]
    public partial class ViewFeedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public int? ProductId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? Status { get; set; }
        public int? CustomerId { get; set; }
        public int? Slike { get; set; }
        public int? Sstart { get; set; }

        public string? FullName { get; set; }
        public string? Avatar { get; set; }
    }
}
