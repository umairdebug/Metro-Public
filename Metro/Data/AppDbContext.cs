using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Metro.Models;

namespace Metro.Data
{
    public class AppDbContext : DbContext
    {
        public IHttpContextAccessor HttpContextAccessor { get; }
        private AppUserViewModel LogInUser { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor)
            : base(options)
        {
            HttpContextAccessor = contextAccessor;
        }

        public DbSet<Metro.Models.AppUser> AppUser { get; set; } = default!;
        public DbSet<Metro.Models.AppRole> AppRole { get; set; }
        public DbSet<Metro.Models.Blog> Blog { get; set; } = default!;
        public DbSet<Metro.Models.Category> Category { get; set; } = default!;
        public DbSet<Metro.Models.Order> Order { get; set; } = default!;
        public DbSet<Metro.Models.Product> Product { get; set; } = default!;
        public DbSet<Metro.Models.ProductImage> ProductImage { get; set; } = default!;
        public DbSet<Metro.Models.ProductSale> ProductSale { get; set; } = default!;
        public DbSet<Metro.Models.LoginHistory> LoginHistory { get; set; } = default!;
        public DbSet<Metro.Models.ShoppingCart> Carts { get; set; } = default!;
        public DbSet<Metro.Models.CartItem> CartItems { get; set; } = default!;

        public AppUserViewModel GetLogInUser()
        {
            if(LogInUser != null) return LogInUser;

            var token = HttpContextAccessor.HttpContext.Request.Cookies[Globals.LoginCookieName]?.ToString();
            if (string.IsNullOrEmpty(token))
            {
                token = HttpContextAccessor.HttpContext.Request.Headers[Globals.LoginCookieName].ToString();
            }
            if (!string.IsNullOrEmpty(token))
            {
                var loginHistory = LoginHistory.Where(m => m.Token == token).FirstOrDefault();
                if (loginHistory == null || loginHistory.ValidTill < DateTime.Now)
                {
                    HttpContextAccessor.HttpContext.Response.Cookies.Delete(Globals.LoginCookieName, new CookieOptions
                    {
                        IsEssential = true,
                    });

                    return null;
                }
                var user = LoginHistory.Where(m => m.Token == token).Select(m => m.User)
                    .Select(m => new AppUserViewModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Email = m.Email,
                        Roles = m.Roles.Select(n => n.Name).ToList()
                    }).FirstOrDefault();

                LogInUser = user;
                return LogInUser;
            }
            return null;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Product>().HasOne(m => m.Category).WithMany(m => m.Product).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Cascade);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var foreignKey in entityType.GetForeignKeys())
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }

            base.OnModelCreating(modelBuilder);
        }

    }
}
