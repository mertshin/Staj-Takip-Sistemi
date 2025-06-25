using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Business.Interfaces;
using Business.DTOs.InternshipDiaryDtos;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using System.Security.Claims;

namespace Starter.Controllers
{
    [Authorize]
    public class StudentPortalController : Controller
    {
        private readonly IInternshipDiaryService _diaryService;
        private readonly IStudentService _studentService;
        private readonly ApplicationDbContext _context;

        public StudentPortalController(IInternshipDiaryService diaryService, IStudentService studentService, ApplicationDbContext context)
        {
            _diaryService = diaryService;
            _studentService = studentService;
            _context = context;
        }

        // GET: StudentPortal
        public async Task<IActionResult> Index()
        {
            // JWT token'dan kullanıcı ID'sini al
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst("Role")?.Value;
            
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim);
            
            // Eğer öğrenci ise, sadece kendi verilerini görebilir
            if (userRole == "Student")
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
                if (student == null)
                {
                    TempData["Error"] = "Öğrenci profili bulunamadı. Lütfen admin ile iletişime geçin.";
                    return View(new List<InternshipDiaryDTO>());
                }
                int studentId = student.Id;
                
                try
                {
                    var diaries = await _diaryService.GetByStudentIdAsync(studentId);
                    ViewBag.StudentId = studentId;
                    return View(diaries);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Hata oluştu: {ex.Message}";
                    return View(new List<InternshipDiaryDTO>());
                }
            }
            else if (userRole == "Advisor" || userRole == "Admin")
            {
                // Danışman ve Admin tüm öğrencilerin verilerini görebilir
                try
                {
                    // Bu sayfada belirli bir öğrenci seçmek için yönlendirme yapılabilir
                    // Şimdilik boş liste döndürüyoruz
                    ViewBag.Message = "Lütfen bir öğrenci seçin.";
                    return View(new List<InternshipDiaryDTO>());
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Hata oluştu: {ex.Message}";
                    return View(new List<InternshipDiaryDTO>());
                }
            }
            else
            {
                return Forbid();
            }
        }

        // GET: StudentPortal/Create
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateDiary()
        {
            try
            {
                // JWT token'dan kullanıcı ID'sini al
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }

                int userId = int.Parse(userIdClaim);
                
                // Öğrenci profilini bul
                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
                if (student == null)
                {
                    TempData["Error"] = "Öğrenci profili bulunamadı. Lütfen admin ile iletişime geçin.";
                    return RedirectToAction(nameof(Index));
                }
                
                int studentId = student.Id;
                
                // DEBUG: Öğrenci ID'yi logla
                System.Console.WriteLine($"[DEBUG] Student ID: {studentId}");
                System.Console.WriteLine($"[DEBUG] Student found: {student.FirstName} {student.LastName}, AdvisorId: {student.AdvisorId}");
                
                // Öğrencinin staj başvurularını kontrol et
                var applications = await _context.InternshipApplications
                    .Where(a => a.StudentId == studentId)
                    .OrderByDescending(a => a.ApplicationDate)
                    .ToListAsync();
                
                System.Console.WriteLine($"[DEBUG] Found {applications.Count} applications for student {studentId}");
                
                InternshipDiaryCreateDTO createDto;
                
                if (applications.Any())
                {
                    // Eğer staj başvurusu varsa, en son başvuruyu kullan
                    var latestApplication = applications.First();
                    createDto = new InternshipDiaryCreateDTO
                    {
                        Date = DateTime.Today,
                        InternshipApplicationId = latestApplication.Id
                    };
                    ViewBag.ApplicationInfo = $"Staj Başvurusu: {latestApplication.InternshipTopic} ({latestApplication.StartDate:dd.MM.yyyy} - {latestApplication.EndDate:dd.MM.yyyy})";
                }
                else
                {
                    System.Console.WriteLine($"[DEBUG] No applications found, creating default application for student {studentId}");
                    
                    // Staj başvurusu yoksa otomatik oluştur
                    var defaultApplication = new InternshipApplication
                    {
                        StudentId = studentId,
                        AdvisorId = student.AdvisorId,
                        ApplicationDate = DateTime.Now,
                        StartDate = DateTime.Today,
                        EndDate = DateTime.Today.AddDays(30), // 30 günlük staj
                        InternshipTopic = "Test Staj Çalışması",
                        Status = Core.Enums.InternshipStatus.Approved,
                        TotalDays = 30,
                        RejectionReason = null,
                        InternshipPlaceId = null // Nullable olduğu için null bırakabiliriz
                    };
                    
                    System.Console.WriteLine($"[DEBUG] Creating application with AdvisorId: {defaultApplication.AdvisorId}");
                    
                    _context.InternshipApplications.Add(defaultApplication);
                    await _context.SaveChangesAsync();
                    
                    System.Console.WriteLine($"[DEBUG] Application created with ID: {defaultApplication.Id}");
                    
                    createDto = new InternshipDiaryCreateDTO
                    {
                        Date = DateTime.Today,
                        InternshipApplicationId = defaultApplication.Id
                    };
                    ViewBag.ApplicationInfo = $"Otomatik Oluşturulan Staj: {defaultApplication.InternshipTopic} ({defaultApplication.StartDate:dd.MM.yyyy} - {defaultApplication.EndDate:dd.MM.yyyy})";
                }
                
                return View(createDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: StudentPortal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateDiary(InternshipDiaryCreateDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(createDto);
                }

                var createdDiary = await _diaryService.CreateAsync(createDto);
                TempData["Success"] = "Staj günlüğü başarıyla kaydedildi. Danışmanınızın onayını bekliyor.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(createDto);
            }
        }

        // GET: StudentPortal/Edit/5
        public async Task<IActionResult> EditDiary(int id)
        {
            try
            {
                var diary = await _diaryService.GetByIdAsync(id);
                
                if (diary == null)
                {
                    TempData["Error"] = "Günlük bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                // Onaylanmış günlükler düzenlenemez
                if (diary.ApprovalStatus == Core.Enums.InternshipStatus.Approved)
                {
                    TempData["Error"] = "Onaylanmış günlükler düzenlenemez";
                    return RedirectToAction(nameof(Index));
                }

                var updateDto = new InternshipDiaryCreateDTO
                {
                    Date = diary.Date,
                    WorkDone = diary.WorkDone,
                    Description = diary.Description,
                    StartTime = diary.StartTime,
                    EndTime = diary.EndTime,
                    InternshipApplicationId = diary.InternshipApplicationId
                };

                ViewBag.DiaryId = id;
                return View("CreateDiary", updateDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: StudentPortal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDiary(int id, InternshipDiaryCreateDTO updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.DiaryId = id;
                    return View("CreateDiary", updateDto);
                }

                var result = await _diaryService.UpdateAsync(id, updateDto);
                
                if (!result)
                {
                    TempData["Error"] = "Günlük güncellenemedi";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = "Staj günlüğü başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                ViewBag.DiaryId = id;
                return View("CreateDiary", updateDto);
            }
        }
    }
} 