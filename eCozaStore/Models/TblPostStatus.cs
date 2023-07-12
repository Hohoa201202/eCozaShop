using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCozaStore.Models
{
    [Table("tblPostStatus")]
    public partial class TblPostStatus
    {

        [Key]
        public int Status { get; set; }
        public string? StatusName { get; set; }
        public string? Description { get; set; }

    }
}
