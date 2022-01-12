using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Helper;
using ShopOnline.Models;
using ShopOnline.ViewModels;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class ProductsController : Controller
    {
        private readonly ShopOnlineContext _context;

        public INotyfService _notyfService { get; }
        public ProductsController(ShopOnlineContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index(int CatID = 0)
        {
            List<Product> products = new List<Product>();
            if(CatID == 0)
            {
                products = await _context.Products.AsNoTracking()
                                        .Include(p => p.CategoryProducts)
                                        .ThenInclude(r => r.Cat)
                                        .ToListAsync();
            }
            else
            {

                products = await _context.Products.AsNoTracking()
                                        .Include(p => p.CategoryProducts)
                                        .ThenInclude(r => r.Cat)
                                        .Where(x => x.CategoryProducts.Any(c => c.CatId == CatID))
                                        .ToListAsync();


            }


            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", CatID);
            return View(products);
        }

        public IActionResult Filter(int CatID = 0)
        {

            var url = $"/Admin/Products?CatID={CatID}";
            if (CatID == 0) url = "/Admin/Products";

            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.AsNoTracking().Include(p => p.CategoryProducts).ThenInclude(r => r.Cat).FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
           

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Price,Description,CatId,Discount,Thumb,Brand,Active,BestSeller,HomeFlag,Alias,Color,SizeS,SizeM,SizeL,SizeXl,Stock")] AddProduct addproduct, IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(addproduct.ProductName) + extension;
                    addproduct.Thumb = await Utilities.UploadFile(fThumb, @"product", image.ToLower());

                }

                if (string.IsNullOrEmpty(addproduct.Thumb)) addproduct.Thumb = "default.jpg";
                addproduct.Alias = Utilities.SEOUrl(addproduct.ProductName);

                Product product = new Product() {
                    ProductName = addproduct.ProductName,
                    Price = addproduct.Price,
                    Description = addproduct.Description,
                    Discount = addproduct.Discount,
                    Thumb = addproduct.Thumb,
                    Brand = addproduct.Brand,
                    Active = addproduct.Active,
                    BestSeller = addproduct.BestSeller,
                    HomeFlag = addproduct.HomeFlag,
                    Alias = addproduct.Alias,
                    Color = addproduct.Color,
                    SizeS = addproduct.SizeS,
                    SizeM = addproduct.SizeM,
                    SizeL = addproduct.SizeL,
                    SizeXl = addproduct.SizeXl,
                    Stock = addproduct.SizeS + addproduct.SizeM + addproduct.SizeL + addproduct.SizeXl
                };


                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                if (addproduct.CatId != null)
                {
                    foreach (var item in addproduct.CatId)
                    {
                        CategoryProduct categoryProduct = new CategoryProduct() { ProductId = product.ProductId, CatId = item };
                        _context.CategoryProducts.Add(categoryProduct);
                        await _context.SaveChangesAsync();
                    }
                }

                _notyfService.Success("Thêm sản phẩm thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(addproduct);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addproduct = await _context.Products.FindAsync(id);
            if (addproduct == null)
            {
                return NotFound();
            }

            AddProduct product = new AddProduct()
            {
                ProductId = addproduct.ProductId,
                ProductName = addproduct.ProductName,
                Price = addproduct.Price,
                Description = addproduct.Description,
                Discount = addproduct.Discount,
                Thumb = addproduct.Thumb,
                Brand = addproduct.Brand,
                Active = addproduct.Active,
                BestSeller = addproduct.BestSeller,
                HomeFlag = addproduct.HomeFlag,
                Alias = addproduct.Alias,
                Color = addproduct.Color,
                SizeS = addproduct.SizeS,
                SizeM = addproduct.SizeM,
                SizeL = addproduct.SizeL,
                SizeXl = addproduct.SizeXl,
                Stock = addproduct.SizeS + addproduct.SizeM + addproduct.SizeL + addproduct.SizeXl
            };

            var productCat = (from c in _context.CategoryProducts
                                where c.ProductId == id
                                select c.CatId).ToList();


            var categories = new SelectList(_context.Categories, "CatId", "CatName").ToList();

            foreach (var item1 in categories)
            {
                foreach (var item2 in productCat)
                {
                    if (item1.Value == item2.ToString())
                    {
                        item1.Selected = true;
                    }
                }
            }

            ViewData["Categories"] = categories;
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Price,Description,CatId,Discount,Thumb,Brand,Active,BestSeller,HomeFlag,Alias,Color,SizeS,SizeM,SizeL,SizeXl,Stock")] AddProduct addproduct, IFormFile fThumb)
        {
            if (id != addproduct.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(addproduct.ProductName) + extension;
                        addproduct.Thumb = await Utilities.UploadFile(fThumb, @"product", image.ToLower());

                    }

                    if (string.IsNullOrEmpty(addproduct.Thumb)) addproduct.Thumb = "default.jpg";
                    addproduct.Alias = Utilities.SEOUrl(addproduct.ProductName);

                    Product product = new Product()
                    {
                        ProductId = addproduct.ProductId,
                        ProductName = addproduct.ProductName,
                        Price = addproduct.Price,
                        Description = addproduct.Description,
                        Discount = addproduct.Discount,
                        Thumb = addproduct.Thumb,
                        Brand = addproduct.Brand,
                        Active = addproduct.Active,
                        BestSeller = addproduct.BestSeller,
                        HomeFlag = addproduct.HomeFlag,
                        Alias = addproduct.Alias,
                        Color = addproduct.Color,
                        SizeS = addproduct.SizeS,
                        SizeM = addproduct.SizeM,
                        SizeL = addproduct.SizeL,
                        SizeXl = addproduct.SizeXl,
                        Stock = addproduct.SizeS + addproduct.SizeM + addproduct.SizeL + addproduct.SizeXl
                    };

                    _context.CategoryProducts.RemoveRange(_context.CategoryProducts.Where(x => x.ProductId == addproduct.ProductId));
                    await _context.SaveChangesAsync();

                    if(addproduct.CatId != null)
                    {
                        foreach (var item in addproduct.CatId)
                        {
                            CategoryProduct categoryProduct = new CategoryProduct() { ProductId = addproduct.ProductId, CatId = item };
                            _context.CategoryProducts.Add(categoryProduct);
                        }
                    }
                    

                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    _notyfService.Warning("Thêm sản phẩm thất bại");
                    if (!ProductExists(addproduct.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                    
                }

                _notyfService.Success("Thêm sản phẩm thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(addproduct);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            try
            {
                _context.Products.Remove(product);
                int changes = await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                _notyfService.Warning("Xóa sản phẩm thất bại");
                return RedirectToAction("Details", new { id = id });

            }
            
            _notyfService.Success("Xóa sản phẩm thành công");
            return RedirectToAction(nameof(Index));
            
        }

        // GET: Admin/Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .FirstOrDefaultAsync(m => m.ProductId == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        // POST: Admin/Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        
    }
}
