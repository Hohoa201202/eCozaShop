using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("tblFeedback")]
    public partial class TblFeedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public int? ProductId { get; set; }

        [Display(Name = "Nội dung đánh giá")]
        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? Status { get; set; }
        public int? CustomerId { get; set; }
        public int? Slike { get; set; }
        public int? Sstart { get; set; }
    }
}
