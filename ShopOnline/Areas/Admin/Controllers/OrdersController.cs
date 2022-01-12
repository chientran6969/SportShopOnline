using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using ShopOnline.ViewModels;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class OrdersController : Controller
    {
        private readonly ShopOnlineContext _context;

        public INotyfService _notyfService { get; }
        public OrdersController(ShopOnlineContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index(int status = 0)
        {
            List<ShippingStatus> statusList = new List<ShippingStatus>{ 
                new ShippingStatus { ID = 1, Name = "Chờ xác nhận" },
                new ShippingStatus { ID = 2, Name = "Đang Đóng gói"},
                new ShippingStatus { ID = 3, Name = "Đang giao hàng"},
                new ShippingStatus { ID = 4, Name = "Hoàn thành"}
            };

            List<Order> orders = new List<Order>();

            if (status != 0)
            {
                
                orders = await _context.Orders.AsNoTracking().Include(o => o.Account).Where(o => o.Status == status).ToListAsync();
            } else
            {
                orders = await _context.Orders.AsNoTracking().Include(o => o.Account).ToListAsync();
            }

            

            ViewData["Trangthai"] = new SelectList(statusList, "ID" , "Name", status);
            return View(orders);
        }

        // loc theo trang thai don hang
        public IActionResult Filter(int statusID = 0)
        {
            var url = $"/Admin/Orders?status={statusID}";
            if (statusID == 0) url = "/Admin/Orders";   

            return Json(new { status = "success", redirectUrl = url });
        }


        // Thay đổi trạng thái đơn hàng
        public  IActionResult Status(int OrderID, int StatusID)
        {
            if (OrderID == 0 || StatusID == 0)
            {
                _notyfService.Warning("Cập nhật trạng thái đơn hàng Thất bại");
                return Json(new { status = "fail" });
            }

            Order order = new Order { OrderId = OrderID, Status = StatusID };

            _context.Orders.Attach(order);
            _context.Entry(order).Property(x => x.Status).IsModified = true;
            var changes = _context.SaveChanges();

            _notyfService.Success("Cập nhật trạng thái đơn hàng thành công");
            return Json(new { success = true });
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Where(o => o.OrderProducts.Any(o => o.OrderId == id))
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        //// GET: Admin/Orders/Create
        //public IActionResult Create()
        //{
        //    ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
        //    return View();
        //}

        //// POST: Admin/Orders/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("OrderId,AccountId,Amount,OrderDate,Status,Deleted,Note")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(order);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", order.AccountId);
        //    return View(order);
        //}

        // GET: Admin/Orders/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", order.AccountId);
        //    return View(order);
        //}

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("OrderId,AccountId,Amount,OrderDate,Status,Deleted,Note")] Order order)
        //{
        //    if (id != order.OrderId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(order);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.OrderId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", order.AccountId);
        //    return View(order);
        //}

        // GET: Admin/Orders/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders
        //        .Include(o => o.Account)
        //        .FirstOrDefaultAsync(m => m.OrderId == id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}

        // POST: Admin/Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var order = await _context.Orders.FindAsync(id);
        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
