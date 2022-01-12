using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class OrderProduct
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public string Size { get; set; }
        public int? Quantity { get; set; }
        public int? Total { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
