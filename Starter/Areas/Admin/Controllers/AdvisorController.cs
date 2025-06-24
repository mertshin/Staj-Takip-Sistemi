using AutoMapper;
using Business.DTOs.AdvisorDtos;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Area("Admin")]
    public class AdvisorsController : Controller
    {
        private readonly IAdvisorService _advisorService;
        private readonly IDepartmentService _departmentService;

        public AdvisorsController(IAdvisorService advisorService, IDepartmentService departmentService)
        {
            _advisorService = advisorService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var advisors = await _advisorService.GetAllAsync();
            return View(advisors);
        }

        public async Task<IActionResult> Details(int id)
        {
            var advisor = await _advisorService.GetByIdAsync(id);
            if (advisor == null) return NotFound();
            return View(advisor);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _departmentService.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdvisorCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await _departmentService.GetAllAsync();
                return View(dto);
            }

            await _advisorService.CreateAsync(dto);
            TempData["Success"] = "Danışman başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var advisor = await _advisorService.GetByIdAsync(id);
            if (advisor == null) return NotFound();

            var updateDto = new AdvisorUpdateDTO
            {
                FirstName = advisor.FirstName,
                LastName = advisor.LastName,
                Title = advisor.Title,
                Email = advisor.Email,
                Phone = advisor.Phone,
                DepartmentId = advisor.DepartmentId
            };

            ViewBag.Departments = await _departmentService.GetAllAsync();
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AdvisorUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await _departmentService.GetAllAsync();
                return View(dto);
            }

            var updated = await _advisorService.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            TempData["Success"] = "Danışman bilgileri güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var advisor = await _advisorService.GetByIdAsync(id);
            if (advisor == null) return NotFound();
            return View(advisor);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _advisorService.DeleteAsync(id);
            TempData["Success"] = "Danışman silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
