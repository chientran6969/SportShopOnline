using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.ViewModels
{
    public class RegisterViewModel
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

        [StringLength(32, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 32 kí tự")]
        [Display(Name = "Mật khẩu xác nhận")]
        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu xác thực")]
        public string ComPassword { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập ngày sinh")]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date, ErrorMessage = "Vui lòng nhập ngày sinh hợp lệ")]
        public DateTime? Birthday { get; set; }

        [MaxLength(100)]
        [Display(Name = "FullName")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; }

        [MaxLength(200)]
        [Display(Name = "Address")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }

        [MaxLength(10)]
        [Display(Name = "Phone")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ")]
        public string Phone { get; set; }
    }
}
