using Metro.Data;
using Metro.Models;
using Microsoft.AspNetCore.Mvc;

namespace Metro.Controllers
{
    public class RemoteValidationController : Controller
    {
        private readonly AppDbContext _context;

        public RemoteValidationController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult CategoryNameCheck(Category obj)
        {
            var isExisting = _context.Category.Where(m => m.Id != obj.Id && m.Name.Trim() == obj.Name.Trim()).Any();
            return Json(!isExisting);
        }
    }
}
