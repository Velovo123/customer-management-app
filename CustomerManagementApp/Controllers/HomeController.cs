using CustomerManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CustomerManagementApp.Controllers
{
    public class HomeController : Controller
    {
        // Home controller is mainly for routing and basic pages
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Customer");  // Redirecting to CustomerController's Index
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
