using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Business.DTOs.UniversityDtos;
using Microsoft.AspNetCore.Authorization;

namespace Starter.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UniversityController : Controller
    {
        private readonly IUniversityService _universityService;

        public UniversityController(IUniversityService universityService)
        {
            _universityService = universityService;
        }

        // GET: University
        public async Task<IActionResult> Index()
        {
            try
            {
                var universities = await _universityService.GetAllAsync();
                return View(universities);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(new List<UniversityDTO>());
            }
        }

        // GET: University/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Geçersiz üniversite ID'si";
                    return RedirectToAction(nameof(Index));
                }

                var university = await _universityService.GetByIdAsync(id);

                if (university == null)
                {
                    TempData["Error"] = "Üniversite bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                return View(university);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: University/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: University/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UniversityCreateDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(createDto);
                }

                var createdUniversity = await _universityService.CreateAsync(createDto);
                TempData["Success"] = "Üniversite başarıyla oluşturuldu";
                return RedirectToAction(nameof(Details), new { id = createdUniversity.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(createDto);
            }
        }

        // GET: University/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Geçersiz üniversite ID'si";
                    return RedirectToAction(nameof(Index));
                }

                var university = await _universityService.GetByIdAsync(id);

                if (university == null)
                {
                    TempData["Error"] = "Üniversite bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                var updateDto = new UniversityUpdateDTO
                {
                    Name = university.Name,
                    Address = university.Address,
                    Phone = university.Phone,
                    Email = university.Email,
                    Website = university.Website
                };

                ViewBag.UniversityId = id;
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: University/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UniversityUpdateDTO updateDto)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Geçersiz üniversite ID'si";
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.UniversityId = id;
                    return View(updateDto);
                }

                var result = await _universityService.UpdateAsync(id, updateDto);

                if (!result)
                {
                    TempData["Error"] = "Üniversite bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = "Üniversite başarıyla güncellendi";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                ViewBag.UniversityId = id;
                return View(updateDto);
            }
        }

        // GET: University/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Geçersiz üniversite ID'si";
                    return RedirectToAction(nameof(Index));
                }

                var university = await _universityService.GetByIdAsync(id);

                if (university == null)
                {
                    TempData["Error"] = "Üniversite bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                return View(university);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: University/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Geçersiz üniversite ID'si";
                    return RedirectToAction(nameof(Index));
                }

                var result = await _universityService.DeleteAsync(id);

                if (!result)
                {
                    TempData["Error"] = "Üniversite bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = "Üniversite başarıyla silindi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}