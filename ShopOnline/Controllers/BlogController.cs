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
    public class BlogController : Controller
    {
        private readonly ShopOnlineContext dbContext;

        public BlogController(ShopOnlineContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index(int? page)
        {
            if (page == null) page = 1;

            var listPost = dbContext.Posts.Include(p => p.Account).OrderByDescending(p => p.PostId).Where(p => p.Active == true);

            int pageSize = 9;

            int pageNumber = (page ?? 1);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPage = Math.Ceiling((double)listPost.Count() / pageSize);
            return View(listPost.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await dbContext.Posts
                .Include(p => p.Account)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null || post.Active==false)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}
