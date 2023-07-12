using System.ComponentModel.DataAnnotations;

namespace eCozaStore.Models
{
    public class AccountLogin
    {
        [Key]
        [MaxLength(10, ErrorMessage ="Số điện thoại phải là 10 số !")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name ="Mật khẩu")]
        [Required(ErrorMessage ="Vui lòng nhập mật khẩu")]
        [MaxLength(300, ErrorMessage ="Mật khẩu tối đa 30 ký tự !")]
        public string Password { get; set; }
    }
}
