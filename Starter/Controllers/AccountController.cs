using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Business.Services;
using Business.DTOs.AuthDtos;
using Core.Enums;

namespace Starter.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(model);

            if (result.Success)
            {
                // JWT token'ı cookie'ye kaydet (güvenlik için HttpOnly)
                Response.Cookies.Append("jwt", result.Token!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // Development için false, production'da true olmalı
                    SameSite = SameSiteMode.Lax, // Strict yerine Lax kullan
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                // Debug: Token ve rol bilgilerini logla
                Console.WriteLine($"Login successful for user: {result.Email}, Role: {result.Role}");
                Console.WriteLine($"JWT Token created: {result.Token?.Substring(0, 50)}...");

                // Role'e göre yönlendirme
                switch (result.Role)
                {
                    case "Admin":
                        Console.WriteLine("Redirecting to Admin area");
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    case "Advisor":
                        return RedirectToAction("Index", "AdvisorPortal");
                    case "Student":
                        return RedirectToAction("Index", "StudentPortal");
                    default:
                        return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", result.Message);
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.RegisterAsync(model);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", result.Message);
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PendingUsers()
        {
            var users = await _authService.GetPendingUsersAsync();
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveUser(int userId)
        {
            var result = await _authService.ApproveUserAsync(userId);
            
            if (result)
                TempData["Success"] = "Kullanıcı başarıyla onaylandı";
            else
                TempData["Error"] = "Kullanıcı onaylanamadı";

            return RedirectToAction("PendingUsers");
        }
    }
} 