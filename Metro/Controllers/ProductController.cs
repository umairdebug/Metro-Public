using Metro.Data;
using Metro.Handlers;
using Metro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Metro.Controllers
{
    [CustomAuthorized]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string categoryId)
        {
            var productQuery = _context.Product.AsQueryable();

            /*if (!string.IsNullOrWhiteSpace(k))
            {
                productQuery = productQuery.Where(m => m.Id.StartsWith(k) || m.Description.StartsWith(k));
            }

            ViewBag.searchUrl = "/Product";
            ViewBag.searchKeyword = k;*/
            var data = productQuery.Where(m => string.IsNullOrEmpty(categoryId) || m.CategoryId == categoryId)
                .Select(m => new ProductViewModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Rank = m.Rank,
                        SalePrice = m.SalePrice,
                        PurchasePrice = m.PurchasePrice,
                        Category = m.Category.Name,
                        ImageUrl = m.Images.OrderBy(m => m.DbEntryTime).Select(m => m.Url).FirstOrDefault()
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
        public IActionResult Create(Product model)
        {
            if (ModelState.IsValid)
            {
                if (model.Uploads?.Any() == true)
                {
                    model.Images ??= new();
                    int imageRank = 0;
                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string appPath = Path.Combine("images", "products");
                    string directryPath = Path.Combine(basePath, appPath);
                    Directory.CreateDirectory(directryPath);

                    foreach (var item in model.Uploads)
                    {
                        if (item.Length > 0)
                        {
                            string fileName = Path.GetRandomFileName().Replace(".", "") + Path.GetExtension(item.FileName);

                            using var stream = new FileStream(Path.Combine(directryPath, fileName), FileMode.Create);
                            item.CopyTo(stream);
                            model.Images.Add(new ProductImage
                            {
                                Rank = ++imageRank,
                                ProductId = model.Id,
                                Url = Path.Combine(appPath, fileName).Replace("\\", "/")
                            });
                        }
                    }

                    _context.Add(model);
                    int r = _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(RedirectToAction(nameof(Index)));
        }

        public IActionResult Edit(string id)
        {
            var product = _context.Product
            .Include(m => m.Images)
            .Include(m => m.Category)
            .FirstOrDefault(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product model, List<string> DeletedImageId)
        {
            if (ModelState.IsValid)
            {
                if (model.Uploads?.Any() == true)
                {
                    List<ProductImage> Images = new();
                    int imageRank = 0;
                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string appPath = Path.Combine("images", "products");
                    string directryPath = Path.Combine(basePath, appPath);
                    Directory.CreateDirectory(directryPath);

                    foreach (var item in model.Uploads)
                    {
                        if (item.Length > 0)
                        {
                            string fileName = Path.GetRandomFileName().Replace(".", "") + Path.GetExtension(item.FileName);
                            using var stream = new FileStream(Path.Combine(directryPath, fileName), FileMode.Create);
                            item.CopyTo(stream);
                            Images.Add(new ProductImage
                            {
                                Rank = ++imageRank,
                                ProductId = model.Id,
                                Url = Path.Combine(appPath, fileName).Replace("\\", "/")
                            });
                        }
                    }

                    var imagesToDelete = _context.ProductImage.Where(m => m.ProductId == model.Id && DeletedImageId.Contains(m.Id)).ToList();

                    _context.AddRange(Images);
                    var imageUrlToDelete = imagesToDelete.Select(m => m.Url).ToList();
                    _context.Update(model);
                    _context.RemoveRange(imagesToDelete);

                    int r = _context.SaveChanges();
                    if (r > 0)
                    {
                        foreach (var url in imageUrlToDelete)
                        {
                            try
                            {
                                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", url));
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        public IActionResult Delete(string id)
        {
            var cat = _context.Product.Find(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            var cat = _context.Product.Find(id);
            if (cat == null) return NotFound();
            _context.Remove(cat);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
