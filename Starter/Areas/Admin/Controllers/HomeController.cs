using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Starter.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Debug: Kullanıcı bilgilerini logla
            Console.WriteLine($"Admin Home Index called");
            Console.WriteLine($"User authenticated: {User.Identity?.IsAuthenticated}");
            Console.WriteLine($"User name: {User.Identity?.Name}");
            
            var claims = User.Claims.ToList();
            Console.WriteLine($"User claims count: {claims.Count}");
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim - Type: {claim.Type}, Value: {claim.Value}");
            }
            
            return View();
        }
    }
}
