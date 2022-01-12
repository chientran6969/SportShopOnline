using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ShopOnline.Extension.Extension;

namespace ShopOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopOnlineContext _context;

        public HomeController(ShopOnlineContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // danh sách sản phẩm mới
            var lsNewProducts = await _context.Products
                                        .AsNoTracking()
                                        .Where(p => p.Active == true)
                                        .OrderByDescending(p => p.ProductId)
                                        .Take(4)
                                        .ToListAsync();

            // danh sách sản phẩm giảm giá
            var lsSaleProducts = await _context.Products
                                        .AsNoTracking()
                                        .Where(p => p.Discount > 0 && p.Active == true)
                                        .OrderByDescending(p => p.Discount)
                                        .Take(4)
                                        .ToListAsync();


            //danh sach san pham ban chay
            var orderProducts = await _context.OrderProducts
                                        .AsNoTracking()
                                        .Include(p => p.Product)
                                        .ToListAsync();

            var lsBestSellerProducts = (from item in orderProducts
                                      group item.Quantity by item.ProductId into g
                                      orderby g.Sum() descending
                                      select g.Key).Take(4);

            List<Product> lsBestSeller = new List<Product>();
            foreach (var item in lsBestSellerProducts)
            {
                lsBestSeller.Add(await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == item));
            }

            ViewData["lsNewProducts"] = lsNewProducts;
            ViewData["lsSaleProducts"] = lsSaleProducts;
            ViewData["lsBestSeller"] = lsBestSeller;
            

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
