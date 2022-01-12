using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public string Thumb { get; set; }
        public string Alias { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? AccountId { get; set; }
        public bool Active { get; set; }

        public virtual Account Account { get; set; }
    }
}
