using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class Product
    {
        public Product()
        {
            CategoryProducts = new HashSet<CategoryProduct>();
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? Price { get; set; }
        public string Description { get; set; }
        public int? Discount { get; set; }
        public string Thumb { get; set; }
        public string Brand { get; set; }
        public bool Active { get; set; }
        public bool BestSeller { get; set; }
        public bool HomeFlag { get; set; }
        public string Alias { get; set; }
        public string Color { get; set; }
        public int? SizeS { get; set; }
        public int? SizeM { get; set; }
        public int? SizeL { get; set; }
        public int? SizeXl { get; set; }
        public int? Stock { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
