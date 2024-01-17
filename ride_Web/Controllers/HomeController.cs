using Microsoft.AspNetCore.Mvc;
using project_rider.Models;
using System.Diagnostics;

namespace project_rider.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}