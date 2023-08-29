using Metro.Data;
using Metro.Handlers;
using Metro.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Metro.Controllers
{
    //[CustomAuthorized]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}