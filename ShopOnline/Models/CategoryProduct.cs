using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class CategoryProduct
    {
        public int CatDetailId { get; set; }
        public int? ProductId { get; set; }
        public int? CatId { get; set; }

        public virtual Category Cat { get; set; }
        public virtual Product Product { get; set; }
    }
}
