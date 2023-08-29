using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Metro.Data;
using Metro.Models;
using Microsoft.AspNetCore.Authorization;
using Metro.Handlers;

namespace Metro.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AppDbContext _context;

        public AccountsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppUser.ToListAsync());
        }


        [CustomAuthorized]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [CustomAuthorized]
        public IActionResult ChangePassword(AppUser model)
        {
            if (model.Password == model.ConformPassword)
            {
                var userId = _context.GetLogInUser().Id;
                var user = _context.AppUser.Where(m => m.Id == userId).FirstOrDefault();

                user.EncryptedPassword = (userId + model.Password).Encrypt();
                _context.SaveChanges();

                _context.HttpContextAccessor.HttpContext.Response.Cookies.Delete(Globals.LoginCookieName, new CookieOptions
                {
                    IsEssential = true
                });

                return RedirectToAction("Index", "Login");
            }
            ModelState.AddModelError("", "bBth paswords not matched.");
            return View(model);
        }




        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.AppUser == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["AppRoleId"] = new SelectList(_context.AppRole, "Id", "Id");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,Password,ConformPassword")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                appUser.EncryptedPassword = (appUser.Id + appUser.Password).Encrypt();
                _context.Add(appUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.AppUser == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUser.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Email,Phone,Password,ConformPassword")] AppUser appUser)
        {
            if (id != appUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(appUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.AppUser == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.AppUser == null)
            {
                return Problem("Entity set 'AppDbContext.AppUser'  is null.");
            }
            var appUser = await _context.AppUser.FindAsync(id);
            if (appUser != null)
            {
                _context.AppUser.Remove(appUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(string id)
        {
          return _context.AppUser.Any(e => e.Id == id);
        }
    }
}
