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
    [Authorize(Roles="Admin,Staff")]
    public class HomeController : Controller
    {
        private readonly ShopOnlineContext _context;

        public INotyfService _notyfService { get; }

        public HomeController(ShopOnlineContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> IndexAsync(int status = 0)
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
            }
            else
            {
                orders = await _context.Orders.AsNoTracking().Include(o => o.Account).ToListAsync();
            }



            ViewData["Trangthai"] = new SelectList(statusList, "ID", "Name", status);
            return View(orders);
        }
    }
}
