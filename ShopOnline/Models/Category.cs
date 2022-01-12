using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class Category
    {
        public Category()
        {
            CategoryProducts = new HashSet<CategoryProduct>();
        }

        public int CatId { get; set; }
        public string CatName { get; set; }
        public string Thumb { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public int? ParentId { get; set; }
        public int? Levels { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
