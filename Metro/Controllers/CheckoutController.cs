using Metro.Data;
using Metro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Metro.Controllers
{
    public class CheckoutController : Controller
    {
        private AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(Order model)
        {
            var userId = _context.GetLogInUser().Id;
            model.AppUsersID = userId;
            var existingCart = _context.Carts.Where(m => m.UserId == userId).Include(m => m.CartItems).FirstOrDefault();

            model.OrderDetail = existingCart.CartItems.Select(m => new OrderDetail
            {
                ProductId = m.ProductId,
                QuantityDemanded = m.Quantity
            }).ToList();

            model.OrderDetail.ForEach(m => m.OrderId = model.Id);
            _context.Add(model);
            _context.RemoveRange(existingCart.CartItems);
            _context.Remove(existingCart);

            int r = _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
