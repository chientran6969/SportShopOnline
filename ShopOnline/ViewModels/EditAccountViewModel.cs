using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.ViewModels
{
    public class EditAccountViewModel
    {
        [Required(ErrorMessage ="Vui lòng nhập họ và tên")]
        public string Fullname { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập địa chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^((09(\d){8})|(086(\d){7})|(088(\d){7})|(089(\d){7})|(01(\d){9}))$", ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ")] 
        public string PhoneNumber { get; set; }
    }
}
