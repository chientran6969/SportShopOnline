using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.ViewModels
{
    public class LoginViewModel
    {
        [MaxLength(100)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập Email hợp lệ")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Vui lòng nhập Email hợp lệ")]
        public string Email { get; set; }

        [StringLength(32, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 32 kí tự")]
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        public string Password { get; set; }
    }
}
