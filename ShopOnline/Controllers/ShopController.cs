using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Extension;
using ShopOnline.Models;
using ShopOnline.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopOnlineContext _context;

        public INotyfService _notyfService { get; }
        public ShopController(ShopOnlineContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        // session sản phẩm trong giỏ hàng
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }

        [Authorize]
        [Route("history")]
        public IActionResult History()
        {
            var accountID = int.Parse(HttpContext.User.FindFirstValue("ID"));
            // lấy id từ session
            var orders = _context.Orders.AsNoTracking().Where(p => p.AccountId == accountID).ToList();

            return View(orders);
        }

        [Authorize]
        [Route("history/{id}")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountID = int.Parse(HttpContext.User.FindFirstValue("ID"));

            var order =  _context.Orders
                                .Include(o => o.OrderProducts)
                                .ThenInclude(op => op.Product)
                                .Where(o => o.OrderProducts.Any(o => o.OrderId == id) && o.AccountId == accountID)
                                .FirstOrDefault(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // cập nhập số sản phẩm trong giỏ hàng
        public IActionResult LoadNumberCart()
        {
            return ViewComponent("NumberCart");
        }

        [Route("cart")]
        public IActionResult Index()
        {
            return View(GioHang);
        }

        
       
        public IActionResult Checkout()
        {


            if (GioHang.Count <= 0)
            {
                _notyfService.Error("Không thể đặt hàng khi giỏ hàng rỗng!");
                return RedirectToAction("Index", "Home");
            }



            //lấy ID của người dùng
            var accountID = HttpContext.User.FindFirstValue("ID");

            if(accountID == null)
            {
                return Redirect("/login?returnurl=/shop/checkout");
            }

            var account = _context.Accounts.FirstOrDefault(a => a.AccountId == int.Parse(accountID));

            CheckoutViewModel checkoutVM = new CheckoutViewModel()
            {
                Address = account.Address,
                PhoneNumber = account.Phone
            };

            ViewBag.FullName = account.FullName;
            ViewData["cart"] = GioHang;
            return View(checkoutVM);
        }


        // đặt hàng
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel orderInfo)
        {
            ViewData["cart"] = GioHang;
            if (GioHang.Count <= 0)
            {
                _notyfService.Error("Không thể đặt hàng khi giỏ hàng rỗng!");
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cart = GioHang;
                    bool availableOrder = true;
                    string outStock = "";


                    //check inventory
                    int i = 0;
                    foreach (var item in GioHang)
                    {
                        var inventory = _context.Products.FirstOrDefault(p => p.ProductId == item.Product.ProductId);
                        bool available = true;

                        if (item.Size == "s")
                        {
                            if (inventory.SizeS - item.Quantity < 0)
                            {
                                available = false;
                            }
                        }
                        else if (item.Size == "m")
                        {
                            if (inventory.SizeM - item.Quantity < 0)
                            {
                                available = false;
                            }
                        }
                        else if (item.Size == "l")
                        {
                            if (inventory.SizeL - item.Quantity < 0)
                            {
                                available = false;
                            }
                        }
                        else if (item.Size == "xl")
                        {
                            if (inventory.SizeXl - item.Quantity < 0)
                            {
                                available = false;
                            }
                        }

                        if (!available)
                        {
                            outStock += item.Product.ProductName + " - Size: " + item.Size + ", ";
                            availableOrder = false;
                            cart.RemoveAt(i);
                            HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                        }
                        i++;
                    }

                    if (!availableOrder)
                    {
                        _notyfService.Warning("Đặt hàng thất bại");
                        ViewBag.outStock = outStock.Remove(outStock.Length - 2, 1);
                        return View(orderInfo);
                    }


                    //var userID = HttpContext.Session.GetString("ID");
                    var accountID = int.Parse(HttpContext.User.FindFirstValue("ID"));
                    var order = new Order()
                    {
                        AccountId = accountID,
                        Amount = (int)cart.Sum(p => p.Amount),
                        OrderDate = DateTime.Now,
                        Status = 1,
                        Note = orderInfo.Note,
                        Address = orderInfo.Address,
                        PhoneNumber = orderInfo.PhoneNumber
                    };

                    _context.Add(order);
                    _context.SaveChanges();

                    foreach (var item in cart)
                    {
                        var inventory = _context.Products.FirstOrDefault(p => p.ProductId == item.Product.ProductId);

                        if (item.Size == "s")
                        {
                            inventory.SizeS = inventory.SizeS - item.Quantity;
                            inventory.Stock = inventory.Stock - item.Quantity;

                            _context.Products.Attach(inventory);
                            _context.Entry(inventory).Property(x => x.SizeS).IsModified = true;
                            _context.Entry(inventory).Property(x => x.Stock).IsModified = true;
                        }
                        else if (item.Size == "m")
                        {
                            inventory.SizeM = inventory.SizeM - item.Quantity;
                            inventory.Stock = inventory.Stock - item.Quantity;

                            _context.Products.Attach(inventory);
                            _context.Entry(inventory).Property(x => x.SizeM).IsModified = true;
                            _context.Entry(inventory).Property(x => x.Stock).IsModified = true;
                        }
                        else if (item.Size == "l")
                        {
                            inventory.SizeL = inventory.SizeL - item.Quantity;
                            inventory.Stock = inventory.Stock - item.Quantity;

                            _context.Products.Attach(inventory);
                            _context.Entry(inventory).Property(x => x.SizeL).IsModified = true;
                            _context.Entry(inventory).Property(x => x.Stock).IsModified = true;
                        }
                        else if (item.Size == "xl")
                        {
                            inventory.SizeXl = inventory.SizeXl - item.Quantity;
                            inventory.Stock = inventory.Stock - item.Quantity;

                            _context.Products.Attach(inventory);
                            _context.Entry(inventory).Property(x => x.SizeXl).IsModified = true;
                            _context.Entry(inventory).Property(x => x.Stock).IsModified = true;
                        }

                        var orderDetail = new OrderProduct()
                        {
                            OrderId = order.OrderId,
                            ProductId = item.Product.ProductId,
                            Size = item.Size,
                            Quantity = item.Quantity,
                            Total = (int)item.Amount
                        };
                        _context.Add(orderDetail);
                    }

                    _context.SaveChanges();

                    //xoa session
                    HttpContext.Session.Remove("GioHang");
                    _notyfService.Success("Đặt hàng thành công");

                    ViewBag.status = "Success";
                    ViewBag.FullName = _context.Accounts.FirstOrDefault(a => a.AccountId == accountID).FullName;
                    return View(orderInfo);
                }
                catch
                {
                    _notyfService.Warning("Đặt hàng thất bại");
                    return View(orderInfo);

                }

            }

            return View(orderInfo);
        }

        //Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult Add(int productID, string size, int quantity)
        {

            if (productID == 0 || quantity <= 0 || size == null)
            {
                _notyfService.Warning("Thêm vào giỏ hàng thất bại");
                return Json(new { success = false });
            }
            else if (size != "s" && size != "m" && size != "l" && size != "xl")
            {
                _notyfService.Warning("Thêm vào giỏ hàng thất bại");
                return Json(new { success = false });
            }


            var sp = _context.Products.FirstOrDefault(p => p.ProductId == productID);

            if (sp == null)
            {
                _notyfService.Warning("Thêm vào giỏ hàng thất bại");
                return Json(new { success = false });
            }

            List<CartItem> gioHang = GioHang;

            try
            {
                CartItem item = gioHang.SingleOrDefault(p => p.Product.ProductId == productID && p.Size == size);

                if (item != null) // đã có trong giỏ hàng
                {
                    item.Quantity += quantity;
                }
                else
                {
                    item = new CartItem
                    {
                        Product = sp,
                        Size = size,
                        Quantity = quantity
                    };
                    gioHang.Add(item);
                }

                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                _notyfService.Success("Thêm vào giỏ hàng thành công");
                return Json(new { success = true });
            }
            catch
            {
                _notyfService.Warning("Thêm vào giỏ hàng thất bại");
                return Json(new { success = false });
            }

            return Json(new { success = false });
        }



        //Cập nhật giỏ hàng
        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult Update(int productID, string size, int quantity)
        {
            if (productID == 0 || quantity <= 0 || size == null)
            {
                _notyfService.Warning("Cập nhật giỏ hàng thất bại");
                return Json(new { success = false });
            }
            else if (size != "s" && size != "m" && size != "l" && size != "xl")
            {
                _notyfService.Warning("Cập nhật giỏ hàng thất bại");
                return Json(new { success = false });
            }

            
            try
            {
                List<CartItem> gioHang = GioHang;
                var item = gioHang.FirstOrDefault(i => i.Product.ProductId == productID);
                if (item == null)
                {
                    return Json(new { success = false });
                }

                // update số lượng
                item.Quantity = quantity;
                var amount = item.Amount;
                var id = item.Product.ProductId;

                // lưu gio hàng lại vào session
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                return Json(new { success = true, totalMoney = GioHang.Sum(p => p.Amount), amount = amount});
            }
            catch
            {
                return Json(new { success = false });
            }
            
        }
        
        //Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        [Route("api/cart/remove")]
        public IActionResult Remove(int productID, string size)
        {
            try
            {
                List<CartItem> gioHang = GioHang;

                CartItem item = gioHang.SingleOrDefault(p => p.Product.ProductId == productID && p.Size == size);

                bool status = false;

                if (item == null)
                {
                    _notyfService.Warning("Xóa sản phẩm thất bại");
                    return Json(new { success = false });
                }

                status = gioHang.Remove(item);


                if (status)
                {
                    _notyfService.Success("Xóa sản phẩm thành công");
                    HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                }
                else
                {
                    _notyfService.Warning("Xóa sản phẩm thất bại");
                }

                return Json(new { success = status, totalMoney = GioHang.Sum(p => p.Amount) });
            }
            catch
            {
                _notyfService.Warning("Xóa sản phẩm thất bại");
                return Json(new { success = false });

            }

        }
    }
}
