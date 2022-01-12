using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.ViewModels
{
    public class AddProduct
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Giá")]
        public int? Price { get; set; }
        public string Description { get; set; }
        public int[] CatId { get; set; }
        public int? Discount { get; set; }
        public string Thumb { get; set; }
        public string Brand { get; set; }
        public bool Active { get; set; }
        public bool BestSeller { get; set; }
        public bool HomeFlag { get; set; }
        public string Alias { get; set; }
        public string Color { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Số lượng SizeS")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng nhập số nguyên dương")]
        public int? SizeS { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Số lượng SizeM")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng nhập số nguyên dương")]
        public int? SizeM { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Số lượng SizeL")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng nhập số nguyên dương")]
        public int? SizeL { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Số lượng SizeXL")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng nhập số nguyên dương")]
        public int? SizeXl { get; set; }
        public int? Stock { get; set; }
    }
}
