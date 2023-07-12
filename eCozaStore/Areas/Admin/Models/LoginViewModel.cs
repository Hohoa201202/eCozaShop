using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace eCozaStore.Areas.Admin.Models
{
    public class LoginViewModel
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage = ("Vui lòng nhập Email"))]
        [Display(Name = "Địa chỉ Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Sai định dạng Email")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(30, ErrorMessage = "Bạn cần đặt mật khẩu tối đa 30 ký tự")]
        public string Password { get; set; }
    }
}
