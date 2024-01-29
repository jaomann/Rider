using Microsoft.AspNetCore.Mvc;
using project_rider.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace project_rider.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(DateTime exp, JwtSecurityTokenHandler t)
        {
            return View();
        }
    }
}