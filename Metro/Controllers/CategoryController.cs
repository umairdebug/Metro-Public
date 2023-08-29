using Metro.Data;
using Metro.Handlers;
using Metro.Models;
using Microsoft.AspNetCore.Mvc;

namespace Metro.Controllers
{
    //[CustomAuthorized(Roles = $"{Globals.UserRole}, {Globals.ShopKeeperRole}")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string k, CategoryType? type)
        {
            if (type == null)
            {
                type = CategoryType.Category;
            }
            ViewBag.type = type;

            var categoryQuery = _context.Category.Where(m => m.Type == type);
            if (!string.IsNullOrWhiteSpace(k))
            {
                categoryQuery = categoryQuery.Where(m => m.Name.StartsWith(k) || m.Description.Contains(k));
            }
            ViewBag.searchUrl = "/Categories";
            var data = categoryQuery.Select(m => new CategoryViewModel
            {
                BrandWiseProducts = m.BrandWiseProducts.Count(),
                CategoryWiseProducts = m.CategoryWiseProducts.Count(),
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                LogoUrl = m.LogoUrl,
                Rank = m.Rank,
                Status = m.Status,
                Type = m.Type
            }).ToList();
            return View(data);
        }
        public IActionResult Create(bool iar)
        {
            if (iar)
            {
                Thread.Sleep(1000);
                return PartialView();
            }

            return View();
        }
        [HttpPost]
        public IActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                if (model.Logo != null && model.Logo.Length > 0)
                {
                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string appPath = Path.Combine("images", "categories");
                    string directryPath = Path.Combine(basePath, appPath);
                    Directory.CreateDirectory(directryPath);
                    string fileName = Path.GetRandomFileName().Replace(".", "") + Path.GetExtension(model.Logo.FileName);

                    using var stream = new FileStream(Path.Combine(directryPath, fileName), FileMode.Create);
                    model.Logo.CopyTo(stream);
                    model.LogoUrl = Path.Combine(appPath, fileName).Replace("\\", "/");
                }
                _context.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Edit(string id)
        {
            var cat = _context.Category.Find(id);
            if (cat == null) return NotFound();
            return View(cat);
        }
        [HttpPost]
        public IActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                if (model.Logo != null && model.Logo.Length > 0)
                {
                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string appPath = Path.Combine("images", "categories");
                    string fileName = Path.GetRandomFileName().Replace(".", "") + Path.GetExtension(model.Logo.FileName);
                    string directryPath = Path.Combine(basePath, appPath);
                    Directory.CreateDirectory(directryPath);

                    using var stream = new FileStream(Path.Combine(directryPath, fileName), FileMode.Create);
                    model.Logo.CopyTo(stream);
                    model.LogoUrl = Path.Combine(appPath, fileName).Replace("\\", "/");
                }
                _context.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Delete(string id)
        {
            var cat = _context.Category.Find(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            var cat = _context.Category.Find(id);
            if (cat == null) return NotFound();
            _context.Remove(cat);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
