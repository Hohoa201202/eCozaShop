using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("tblTransactStatus")]
    public partial class TblTransactStatus
    {

        [Key]
        public int TransactStatusId { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }

    }
}
