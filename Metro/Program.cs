using Microsoft.EntityFrameworkCore;
using Metro.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddHttpContextAccessor();
string connectionString = builder.Configuration.GetConnectionString("AppDbContext");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDistributedSqlServerCache(m =>
{
    m.ConnectionString = connectionString;
    m.SchemaName = "dbo";
    m.TableName = "SessionData";
});

builder.Services.AddSession(m => { m.IdleTimeout = TimeSpan.FromMinutes(20); });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using var scope = app.Services.CreateScope();
var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
_context.Database.Migrate();

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
