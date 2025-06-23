using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Business.Interfaces;
using Business.DTOs.DepartmentDtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Starter.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IUniversityService _universityService;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService, IUniversityService universityService, IMapper mapper)
        {
            _departmentService = departmentService;
            _universityService = universityService;
            _mapper = mapper;
        }

        // GET: Department
        public async Task<IActionResult> Index(int? universityId)
        {
            try
            {
                IEnumerable<DepartmentDTO> departments;

                if (universityId.HasValue)
                {
                    departments = await _departmentService.GetByUniversityIdAsync(universityId.Value);
                    ViewBag.FilteredUniversityId = universityId.Value;
                }
                else
                {
                    departments = await _departmentService.GetAllAsync();
                }

                // Üniversite dropdown için - DepartmentService üzerinden
                var universities = await _departmentService.GetUniversitiesForDropdownAsync();
                ViewBag.Universities = new SelectList(universities, "Id", "Name", universityId);

                return View(departments);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(new List<DepartmentDTO>());
            }
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Geçersiz bölüm ID'si";
                    return RedirectToAction(nameof(Index));
                }

                var department = await _departmentService.GetByIdAsync(id);

                if (department == null)
                {
                    TempData["Error"] = "Bölüm bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                return View(department);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Department/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                await LoadUniversitiesDropdown();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentCreateDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadUniversitiesDropdown(createDto.UniversityId);
                    return View(createDto);
                }

                var createdDepartment = await _departmentService.CreateAsync(createDto);
                TempData["Success"] = "Bölüm başarıyla oluşturuldu";
                return RedirectToAction(nameof(Details), new { id = createdDepartment.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                await LoadUniversitiesDropdown(createDto.UniversityId);
                return View(createDto);
            }
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Geçersiz bölüm ID'si";
                    return RedirectToAction(nameof(Index));
                }

                var department = await _departmentService.GetByIdAsync(id);

                if (department == null)
                {
                    TempData["Error"] = "Bölüm bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                var updateDto = _mapper.Map<DepartmentUpdateDTO>(department);

                await LoadUniversitiesDropdown(department.UniversityId);
                ViewBag.DepartmentId = id;
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentUpdateDTO updateDto)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Geçersiz bölüm ID'si";
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid)
                {
                    await LoadUniversitiesDropdown(updateDto.UniversityId);
                    ViewBag.DepartmentId = id;
                    return View(updateDto);
                }

                var result = await _departmentService.UpdateAsync(id, updateDto);

                if (!result)
                {
                    TempData["Error"] = "Bölüm bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = "Bölüm başarıyla güncellendi";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                await LoadUniversitiesDropdown(updateDto.UniversityId);
                ViewBag.DepartmentId = id;
                return View(updateDto);
            }
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Geçersiz bölüm ID'si";
                    return RedirectToAction(nameof(Index));
                }

                var department = await _departmentService.GetByIdAsync(id);

                if (department == null)
                {
                    TempData["Error"] = "Bölüm bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                return View(department);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Geçersiz bölüm ID'si";
                    return RedirectToAction(nameof(Index));
                }

                var result = await _departmentService.DeleteAsync(id);

                if (!result)
                {
                    TempData["Error"] = "Bölüm bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = "Bölüm başarıyla silindi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper metod - Üniversite dropdown yüklemek için
        private async Task LoadUniversitiesDropdown(int? selectedUniversityId = null)
        {
            // DepartmentService üzerinden universities alınıyor
            var universities = await _departmentService.GetUniversitiesForDropdownAsync();
            ViewBag.UniversityId = new SelectList(universities, "Id", "Name", selectedUniversityId);
        }
    }
}