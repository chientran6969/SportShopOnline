using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Helper;
using ShopOnline.Models;
using ShopOnline.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ShopOnline.Controllers
{
    public class AccountController : Controller
    {
        private readonly ShopOnlineContext dbContext;

        public INotyfService _notyfService { get; }

        public AccountController(ShopOnlineContext dbContext, INotyfService notyfService)
        {
            this.dbContext = dbContext;
            this._notyfService = notyfService;
        }

        
        [Route("Login")]
        public IActionResult Login(string returnurl = null)
        {
            var accountID = HttpContext.User.FindFirstValue("ID");

            if (accountID != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.returnurl = returnurl;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel account, string returnurl = null)
        {
            var accountID = HttpContext.Session.GetString("ID");

            if (accountID != null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var user = dbContext.Accounts.AsNoTracking().SingleOrDefault(a => a.Email.Trim() == account.Email);


                if (user == null || user.Active == false || Utilities.ToMD5(account.Password) != user.Password)
                {
                    TempData["fail"] = "Tài khoản hoặc mật khẩu không đúng!";
                    return RedirectToAction("Login");
                }
                

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("ID", user.AccountId.ToString()));
                claims.Add(new Claim("FullName", user.FullName));
                claims.Add(new Claim("Email", account.Email));
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPricipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPricipal);

                if (!string.IsNullOrEmpty(returnurl))
                    return Redirect(returnurl);
                else if (user.Role == "Customer")
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

            }
            return View(account);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _notyfService.Success("Đã đăng xuất thành công");
            return RedirectToAction("Index", "Home");
        }

        [Route("Register")]
        public IActionResult Register()
        {
            var accountID = HttpContext.User.FindFirstValue("ID");

            if (accountID != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel account)
        {
            if (ModelState.IsValid)
            {
                if (account.Birthday > DateTime.Today)
                {
                    TempData["fail"] = "Ngày sinh phải ở quá khứ";
                    return View(account);
                }
                if (!account.Password.Equals(account.ComPassword))
                {
                    TempData["fail"] = "Mật khẩu xác thực không trùng khớp";
                    return View(account);
                }

                var user = dbContext.Accounts.AsNoTracking().SingleOrDefault(a => a.Email.Trim() == account.Email);
                if(user != null)
                {
                    TempData["fail"] = "Email đăng kí đã tồn tại";
                    return View(account);
                }

                account.Password = Utilities.ToMD5(account.Password);

                Account acc = new Account();
                acc.Email = account.Email;
                acc.Password = account.Password;
                acc.FullName = account.FullName;
                acc.Address = account.Address;
                acc.Phone = account.Phone;
                acc.Birthday = account.Birthday;
                acc.Role = "Customer";
                acc.CreateAt = DateTime.Today;

                dbContext.Accounts.Add(acc);

                await dbContext.SaveChangesAsync();

                _notyfService.Success("Đăng kí thành công",3);

                return RedirectToAction("Login");

            }
            return View(account);
        }


        public IActionResult Edit()
        {
            var accountID = HttpContext.User.FindFirstValue("ID");

            var account = dbContext.Accounts.AsNoTracking().FirstOrDefault(a => a.AccountId == int.Parse(accountID));

            EditAccountViewModel editAccountViewModel = new EditAccountViewModel()
            {
                Fullname = account.FullName,
                Address = account.Address,
                PhoneNumber = account.Phone
            };

            return View(editAccountViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditAccountViewModel account)
        {
            var accountID = HttpContext.User.FindFirstValue("ID");

            if (ModelState.IsValid)
            {
                try
                {
                    Account user = new Account() { AccountId = int.Parse(accountID), FullName = account.Fullname, Address = account.Address, Phone = account.PhoneNumber };
                    dbContext.Accounts.Attach(user);
                    dbContext.Entry(user).Property(x => x.FullName ).IsModified = true;
                    dbContext.Entry(user).Property(x => x.Address ).IsModified = true;
                    dbContext.Entry(user).Property(x => x.Phone ).IsModified = true;
                    var change = dbContext.SaveChanges();
                    if (change > 0)
                    {
                        _notyfService.Success("Cập nhật thành công");
                    }
                    else
                    {
                        _notyfService.Error("Cập nhật thất bại");
                    }

                }
                catch
                {
                    _notyfService.Error("Cập nhật thất bại");
                }

            }


            return View(account);
        }

        
        [HttpPost]
        public IActionResult ChangePassword(string Old, string New)
        {
            var accountID = int.Parse(HttpContext.User.FindFirstValue("ID"));

            if (string.IsNullOrEmpty(Old) || string.IsNullOrEmpty(New))
            {
                TempData["fail"] = "Vui lòng nhập đầy đủ thông tin";
                return RedirectToAction("Edit");
            }
            else if (New.Length < 6 || New.Length > 32)
            {
                TempData["fail"] = "Mật khẩu phải từ 6 đến 32 kí tự";
                return RedirectToAction("edit");
            }

            try
            {
                var md5Old = Utilities.ToMD5(Old);
                var account = dbContext.Accounts.AsNoTracking().FirstOrDefault(a => a.AccountId == accountID && a.Password == md5Old);
                if (account == null)
                {
                    TempData["fail"] = "Mật khẩu cũ không đúng";
                    return RedirectToAction("edit");
                }

                var md5New = Utilities.ToMD5(New);
                Account user = new Account() { AccountId = accountID, Password = md5New };
                dbContext.Accounts.Attach(user);
                dbContext.Entry(user).Property(x => x.Password).IsModified = true;
                var change = dbContext.SaveChanges();
                if (change > 0)
                {
                    _notyfService.Success("Đổi mật khẩu thành công");
                }
                else
                {
                    _notyfService.Error("Đổi mật khảu thất bại");
                }
                

            }
            catch
            {
                _notyfService.Error("Đổi mật khẩu thất bại");
            }
            
            return RedirectToAction("Edit");
        }
    }
}
