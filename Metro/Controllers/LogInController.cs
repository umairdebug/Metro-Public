using Metro.Data;
using Metro.Models;
using Microsoft.AspNetCore.Mvc;

namespace Metro.Controllers
{
    public class LogInController : Controller
    {
        private readonly AppDbContext _context;

        public LogInController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            var user = _context.AppUser.Where(m => m.Email == model.Email).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email address not exists");
                return View(model);
            }

            if ((user.Id + model.Password).Encrypt() == user.EncryptedPassword)
            {
                LoginHistory loginHistory = new()
                {
                    ClientInfo = _context.HttpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString(),
                    IPAddress = _context.HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    UserId = user.Id,
                    ValidTill = model.RememberMe ? DateTime.Now.AddDays(3) : DateTime.Now.AddMinutes(1000)
                };

                _context.Add(loginHistory);
                _context.SaveChanges();

                HttpContext.Response.Cookies.Append(Globals.LoginCookieName, loginHistory.Token, new CookieOptions
                {
                    IsEssential = true,
                    Expires = loginHistory.ValidTill,
                });
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("Password", "Invalid password");
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(Globals.LogInSessionName);
            return RedirectToAction("Index", "Home");
        }
    }
}
