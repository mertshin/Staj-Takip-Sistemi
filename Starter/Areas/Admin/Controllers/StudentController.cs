using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Business.Interfaces;
using Business.DTOs.StudentDtos;
using AutoMapper;

namespace Starter.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IDepartmentService _departmentService;
        private readonly IAdvisorService _advisorService;
        private readonly IMapper _mapper;

        public StudentController(IStudentService studentService, IDepartmentService departmentService, 
                                IAdvisorService advisorService, IMapper mapper)
        {
            _studentService = studentService;
            _departmentService = departmentService;
            _advisorService = advisorService;
            _mapper = mapper;
        }

        // GET: Student
        public async Task<IActionResult> Index(int? departmentId)
        {
            try
            {
                IEnumerable<StudentDTO> students;

                if (departmentId.HasValue)
                {
                    students = await _studentService.GetByDepartmentIdAsync(departmentId.Value);
                    ViewBag.FilteredDepartmentId = departmentId.Value;
                }
                else
                {
                    students = await _studentService.GetAllAsync();
                }

                var departments = await _studentService.GetDepartmentsForDropdownAsync();
                ViewBag.Departments = new SelectList(departments, "Id", "Name", departmentId);

                return View(students);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(new List<StudentDTO>());
            }
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Geçersiz öğrenci ID'si";
                    return RedirectToAction(nameof(Index));
                }

                var student = await _studentService.GetByIdAsync(id);

                if (student == null)
                {
                    TempData["Error"] = "Öğrenci bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                return View(student);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Student/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                await LoadDropdownData();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadDropdownData(createDto.DepartmentId, createDto.AdvisorId);
                    return View(createDto);
                }

                var createdStudent = await _studentService.CreateAsync(createDto);
                TempData["Success"] = "Öğrenci başarıyla oluşturuldu";
                return RedirectToAction(nameof(Details), new { id = createdStudent.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                await LoadDropdownData(createDto.DepartmentId, createDto.AdvisorId);
                return View(createDto);
            }
        }

        private async Task LoadDropdownData(int? selectedDepartmentId = null, int? selectedAdvisorId = null)
        {
            var departments = await _studentService.GetDepartmentsForDropdownAsync();
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", selectedDepartmentId);
            
            if (selectedDepartmentId.HasValue)
            {
                var advisors = await _studentService.GetAdvisorsByDepartmentIdAsync(selectedDepartmentId.Value);
                ViewBag.AdvisorId = new SelectList(advisors, "Id", "Name", selectedAdvisorId);
            }
            else
            {
                ViewBag.AdvisorId = new SelectList(new List<dynamic>(), "Id", "Name");
            }
        }
    }
} 