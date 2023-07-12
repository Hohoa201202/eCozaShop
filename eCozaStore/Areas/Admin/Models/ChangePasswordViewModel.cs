using System.ComponentModel.DataAnnotations;

namespace eCozaStore.Areas.Admin.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Mật khẩu hiện tại")]
        public string PasswordNow { get; set; }
        
        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiếu 6 ký tự")]
        public string Password { get; set; }


        [MinLength(6, ErrorMessage = "Mật khẩu tối thiếu 6 ký tự")]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("Password", ErrorMessage = "Mật khẩu mới không trùng khớp")]
        public string ConfirmPassword { get; set; } 
    
    }
}
