using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_News.Models;

namespace MVC_News.Controllers
{
    [Authorize(Roles ="admin")]
    public class CategoryController : Controller
    {
        private readonly NewsDbContext dbContext;
        public CategoryController(NewsDbContext db)
        {
            dbContext = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await dbContext.Categories.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(category);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id==null || dbContext.Categories==null)
            {
                return NotFound();
            }
            var category = await dbContext.Categories.FindAsync(id);
            if (category==null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id!=category.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(category);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || dbContext.Categories == null)
            {
                return NotFound();
            }
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.Categories == null) 
            { 
                return Problem("Entity set 'NewsDbContext.Categories'  is null."); 
            }
            var category = await dbContext.Categories.FindAsync(id);
            if (category != null)
            {
                dbContext.Categories.Remove(category);
            }
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        private bool CategoryExists(int id)
        {
            return dbContext.Categories.Any(c => c.Id == id);
        }
    }
}
