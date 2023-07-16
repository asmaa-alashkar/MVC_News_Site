using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_News.Models;

namespace MVC_News.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsDbContext db ;
        public NewsController(NewsDbContext dbContext)
        {
            db = dbContext;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            var newsItems = db.NewsItems.Include(n => n.Category).ToList();
            return View(newsItems);
        }
        [HttpPost]
        public IActionResult Index(int? CategoryId)
        {
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            if (CategoryId == null)
                return View(db.NewsItems.Include(n => n.Category).ToList());
            else
            {
                var category = db.Categories.Include(i => i.NewsItems).FirstOrDefault(c => c.Id == CategoryId);
                return View(category.NewsItems.ToList());
            }
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var model = db.NewsItems.FirstOrDefault(n => n.Id == id);
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(NewsItem newsItem,IFormFile ImgSrc)
        {
            if (ModelState.IsValid)
            {
                newsItem.CreatedDate = DateTime.Now;
                db.Add(newsItem);
                await db.SaveChangesAsync();
                if (ImgSrc != null)
                {
                    string imgName = newsItem.Id.ToString() + "." + ImgSrc.FileName.Split(".").Last();
                    using (var obj = new FileStream(@".\wwwroot\images\" + imgName, FileMode.Create))
                    {
                        await ImgSrc.CopyToAsync(obj);
                        newsItem.ImgSrc = imgName;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name", newsItem.CategoryId);
            return View(newsItem);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || db.NewsItems == null)
            {
                return NotFound();
            }
            var newsItem = await db.NewsItems.FindAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            return View(newsItem);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, NewsItem newsItem,IFormFile ImgSrc)
        {
            if (id != newsItem.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    newsItem.CreatedDate = DateTime.Now;
                    db.Update(newsItem);
                    await db.SaveChangesAsync();
                    if (ImgSrc != null)
                    {
                        string imgName = newsItem.Id.ToString() + "." + ImgSrc.FileName.Split(".").Last();
                        using (var obj = new FileStream(@".\wwwroot\images\" + imgName, FileMode.Create))
                        {
                            await ImgSrc.CopyToAsync(obj);
                            newsItem.ImgSrc = imgName;
                            await db.SaveChangesAsync();
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsItemExists(newsItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name", newsItem.CategoryId);
            return View(newsItem);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || db.NewsItems == null)
            {
                return NotFound();
            }
            var newsItem = await db.NewsItems.FirstOrDefaultAsync(n => n.Id == id);
            if (newsItem == null)
            {
                return NotFound();
            }
            return View(newsItem);
        }
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.NewsItems == null)
            {
                return Problem("Entity set 'NewsDbContext.NewsItems'  is null.");
            }
            var newsItem = await db.NewsItems.FindAsync(id);
            if (newsItem != null)
            {
                db.NewsItems.Remove(newsItem);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        private bool NewsItemExists(int id)
        {
            return db.NewsItems.Any(n => n.Id == id);
        }

    }
}
