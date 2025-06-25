using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Starter.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUniversityService _universityService;
        private readonly IStudentService _studentService;
        private readonly IInternshipDiaryService _diaryService;
        private readonly IAdvisorService _advisorService;

        public HomeController(
            IUniversityService universityService,
            IStudentService studentService,
            IInternshipDiaryService diaryService,
            IAdvisorService advisorService)
        {
            _universityService = universityService;
            _studentService = studentService;
            _diaryService = diaryService;
            _advisorService = advisorService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Dashboard için istatistikler
                var universities = await _universityService.GetAllAsync();
                var students = await _studentService.GetAllAsync();
                var advisors = await _advisorService.GetAllAsync();
                var diaries = await _diaryService.GetAllAsync();

                ViewBag.UniversityCount = universities.Count();
                ViewBag.StudentCount = students.Count();
                ViewBag.AdvisorCount = advisors.Count();
                ViewBag.TotalDiaryCount = diaries.Count();
                ViewBag.PendingDiaryCount = diaries.Count(d => d.ApprovalStatus == Core.Enums.InternshipStatus.Pending);
                ViewBag.ApprovedDiaryCount = diaries.Count(d => d.ApprovalStatus == Core.Enums.InternshipStatus.Approved);

                return View();
            }
            catch (Exception)
            {
                // İlk çalıştırmada veriler yoksa varsayılan değerler
                ViewBag.UniversityCount = 0;
                ViewBag.StudentCount = 0;
                ViewBag.AdvisorCount = 0;
                ViewBag.TotalDiaryCount = 0;
                ViewBag.PendingDiaryCount = 0;
                ViewBag.ApprovedDiaryCount = 0;

                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Debug action - Authentication durumunu kontrol et
        public IActionResult Debug()
        {
            ViewBag.IsAuthenticated = User.Identity?.IsAuthenticated ?? false;
            ViewBag.UserName = User.Identity?.Name ?? "Anonymous";
            ViewBag.Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            
            // Console'a da yazdır
            Console.WriteLine($"Debug Action - User authenticated: {User.Identity?.IsAuthenticated}");
            Console.WriteLine($"User name: {User.Identity?.Name}");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }
            
            return View();
        }

        [Route("Home/Error")]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                Response.StatusCode = statusCode.Value;
            }
            
            return View();
        }
    }
}
