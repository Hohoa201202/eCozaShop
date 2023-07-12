using System.ComponentModel.DataAnnotations;

namespace eCozaStore.Models
{
    public class ChangePassword
    {
        [Key]
        public int CustomerID { get; set; }

        [Display(Name ="Mật khẩu hiện tại")]
        public string PasswordNow { get; set; }

        [Display(Name ="Mật khẩu mới")]
        [Required(ErrorMessage ="Vui lòng nhập mật khẩu")]
        [MinLength(6, ErrorMessage ="Yêu cầu mật khẩu tối thiểu 6 ký tự")]
        public string Password { get; set; }

        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("Password", ErrorMessage ="Mật khẩu không khớp nhau")]
        public string ConfirmPassword   { get; set; }
    }
}
