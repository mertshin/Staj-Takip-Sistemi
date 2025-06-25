using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Business.Interfaces;
using Business.DTOs.InternshipDiaryDtos;
using Core.Enums;
using System.Security.Claims;

namespace Starter.Controllers
{
    [Authorize(Roles = "Advisor,Admin")]
    public class AdvisorPortalController : Controller
    {
        private readonly IInternshipDiaryService _diaryService;
        private readonly IAdvisorService _advisorService;

        public AdvisorPortalController(IInternshipDiaryService diaryService, IAdvisorService advisorService)
        {
            _diaryService = diaryService;
            _advisorService = advisorService;
        }

        // GET: AdvisorPortal
        public async Task<IActionResult> Index()
        {
            // Burada normalde authentication sistemi olacak
            int advisorId = 1; // Gerçek uygulamada Session/Claims'den gelecek
            
            try
            {
                var pendingDiaries = await _diaryService.GetByAdvisorIdForApprovalAsync(advisorId);
                ViewBag.AdvisorId = advisorId;
                ViewBag.PendingCount = pendingDiaries.Count();
                return View(pendingDiaries);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(new List<InternshipDiaryDTO>());
            }
        }

        // GET: AdvisorPortal/Review/5
        public async Task<IActionResult> ReviewDiary(int id)
        {
            try
            {
                var diary = await _diaryService.GetByIdAsync(id);
                
                if (diary == null)
                {
                    TempData["Error"] = "Günlük bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                if (diary.ApprovalStatus != InternshipStatus.Pending)
                {
                    TempData["Error"] = "Bu günlük zaten değerlendirilmiş";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.DiaryId = id;
                return View(diary);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AdvisorPortal/Approve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveDiary(int diaryId)
        {
            try
            {
                int advisorId = 1; // Gerçek uygulamada Session/Claims'den gelecek
                
                var approvalDto = new InternshipDiaryApprovalDTO
                {
                    DiaryId = diaryId,
                    Decision = InternshipStatus.Approved,
                    AdvisorId = advisorId
                };

                var result = await _diaryService.ApproveAsync(approvalDto);
                
                if (result)
                {
                    TempData["Success"] = "Staj günlüğü başarıyla onaylandı";
                }
                else
                {
                    TempData["Error"] = "Onay işlemi başarısız";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AdvisorPortal/Reject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectDiary(int diaryId, string rejectionReason)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rejectionReason))
                {
                    TempData["Error"] = "Red sebebi belirtilmelidir";
                    return RedirectToAction("ReviewDiary", new { id = diaryId });
                }

                int advisorId = 1; // Gerçek uygulamada Session/Claims'den gelecek
                
                var approvalDto = new InternshipDiaryApprovalDTO
                {
                    DiaryId = diaryId,
                    Decision = InternshipStatus.Rejected,
                    RejectionReason = rejectionReason,
                    AdvisorId = advisorId
                };

                var result = await _diaryService.RejectAsync(approvalDto);
                
                if (result)
                {
                    TempData["Success"] = "Staj günlüğü reddedildi";
                }
                else
                {
                    TempData["Error"] = "Red işlemi başarısız";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: AdvisorPortal/AllDiaries
        public async Task<IActionResult> AllDiaries()
        {
            try
            {
                int advisorId = 1; // Gerçek uygulamada Session/Claims'den gelecek
                
                // Tüm günlükleri getir (sadece onay bekleyenler değil)
                var allDiaries = await _diaryService.GetAllAsync();
                
                // Sadece bu danışmanın öğrencilerinin günlüklerini filtrele
                // Bu kısım için service'e yeni bir metod eklemek daha iyi olur
                // Şimdilik tüm günlükleri döndürelim
                
                return View(allDiaries);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(new List<InternshipDiaryDTO>());
            }
        }
    }
} 