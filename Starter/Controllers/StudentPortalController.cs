using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Business.DTOs.InternshipDiaryDtos;

namespace Starter.Controllers
{
    public class StudentPortalController : Controller
    {
        private readonly IInternshipDiaryService _diaryService;
        private readonly IStudentService _studentService;

        public StudentPortalController(IInternshipDiaryService diaryService, IStudentService studentService)
        {
            _diaryService = diaryService;
            _studentService = studentService;
        }

        // GET: StudentPortal
        public async Task<IActionResult> Index()
        {
            // Burada normalde kullanıcı authentication sistemi olacak
            // Şimdilik test için sabit bir öğrenci ID kullanıyoruz
            int studentId = 1; // Gerçek uygulamada Session/Claims'den gelecek
            
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

        // GET: StudentPortal/Create
        public async Task<IActionResult> CreateDiary()
        {
            try
            {
                // Öğrencinin aktif staj başvurusunu bul
                int studentId = 1; // Gerçek uygulamada Session/Claims'den gelecek
                
                // Bu metodu IStudentService'e eklemek gerekecek
                // var activeApplications = await _studentService.GetActiveInternshipApplicationsAsync(studentId);
                
                // Şimdilik basit bir model döndürelim
                var createDto = new InternshipDiaryCreateDTO
                {
                    Date = DateTime.Today,
                    InternshipApplicationId = 1 // Test için sabit değer
                };
                
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