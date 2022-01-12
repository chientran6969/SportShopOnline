using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopOnlineContext _context;

        public ProductController(ShopOnlineContext context)
        {
            _context = context;
        }

        // danh sách sản phẩm
        public async Task<IActionResult> Index(int catID = 0, int min = 0, int max = 5000000, int page = 1, string search = null)
        {
            var pageNumber = page <= 0 ? 1 : page;
            var pageSize = 9;

            var products = _context.Products
                                        .AsNoTracking()
                                        .Where(p => p.Active == true)
                                        .OrderByDescending(p => p.ProductId);

            if (min != 0 || max != 5000000)
            {
                products = _context.Products
                                        .AsNoTracking()
                                        .Where(p => p.Active == true && p.Price > min && p.Price < max)
                                        .OrderByDescending(p => p.ProductId);


            }
            else
            {
                if (catID == 0)
                {
                    products = _context.Products
                                        .AsNoTracking()
                                        .Where(p => p.Active == true)
                                        .OrderByDescending(p => p.ProductId);
                                        
                }
                else
                {
                    products = _context.Products
                                            .AsNoTracking()
                                            .Where(x => x.CategoryProducts.Any(c => c.CatId == catID) && x.Active == true)
                                            .OrderByDescending(p => p.ProductId);
                                            
                }
            }

            PagedList<Product> productsPage = new PagedList<Product>(products, pageNumber, pageSize);
            ViewBag.TotalPage = Math.Ceiling((double)products.Count() / pageSize);

            // nếu tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                var lsSearch = products.Where(p => p.ProductName.ToLower().Contains(search));
                productsPage = new PagedList<Product>(lsSearch, pageNumber, pageSize);
                ViewBag.TotalPage = Math.Ceiling((double)productsPage.Count() / pageSize);
            }
           
            
            ViewBag.CurrentPage = pageNumber;
            

            var categories = await _context.Categories
                                           .AsNoTracking()
                                           .Where(x => x.Active == true)
                                           .ToListAsync();

            ViewData["categories"] = categories;
            ViewBag.SelectedCat = catID;


            return View(productsPage);
        }

        // chi tiết sản phẩm
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                                        .AsNoTracking()
                                        .Include(p => p.CategoryProducts)
                                        .ThenInclude(op => op.Cat)
                                        .FirstOrDefaultAsync(m => m.ProductId == id && m.Active == true);

            if (product == null)
            {
                return NotFound();
            }

            var related = product.CategoryProducts.FirstOrDefault();

            var relatedProduct = await _context.Products.AsNoTracking()
                                        .Where(p => p.Active == true && p.CategoryProducts.Any(c => c.CatId == related.CatId) && p.ProductId != product.ProductId)
                                        .Take(4)
                                        .ToListAsync();

            ViewData["relatedProduct"] = relatedProduct;

            return View(product);
        }
    }
}
