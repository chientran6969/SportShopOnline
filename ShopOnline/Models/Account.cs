using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class Account
    {
        public Account()
        {
            Orders = new HashSet<Order>();
            Posts = new HashSet<Post>();
        }

        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
