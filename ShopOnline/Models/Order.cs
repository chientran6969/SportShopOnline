using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int OrderId { get; set; }
        public int? AccountId { get; set; }
        public int? Amount { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? Status { get; set; }
        public bool? Deleted { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
