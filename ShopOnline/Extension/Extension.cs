using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Extension
{
    public static class Extension
    {
        public static string toVND(this int price)
        {
            return price.ToString("#,##0") + "đ";
        }

        public static double Round(double value)
        {
            double n = Math.Pow(10, -5);
            return Math.Round(value / n, 0) * n;
        }
    }
}
