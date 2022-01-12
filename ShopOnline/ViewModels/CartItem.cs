using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.ViewModels
{
    public class CartItem
    {
        public Product Product { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public double Price => (double)(Math.Round((double)(Product.Price - (Product.Price * Product.Discount / 100)) / 10000) * 10000);

        public double Amount => (double)(Quantity * Price);

        //public static double CaculatePrice(double price)
        //{
        //    var priceBefore = Product.Price - (Product.Price * Product.Discount / 100);
        //    var priceAfter = Math.Round(Convert.ToDouble(priceBefore) / 10000) * 10000;
        //    return Math.Round(Convert.ToDouble(price) / 10000) * 10000;
        //}
    }
}
