using Business.DTOs.AdvisorDtos;
using Business.DTOs.DepartmentDtos;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ViewModels;
namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUniversityService _universityService;
        private readonly IDepartmentService _departmentService;
        private readonly IAdvisorService _advisorService;

        public HomeController(IUniversityService universityService,
                              IDepartmentService departmentService,
                              IAdvisorService advisorService)
        {
            _universityService = universityService;
            _departmentService = departmentService;
            _advisorService = advisorService;
        }

        public async Task<IActionResult> Index(int? universityId, int? departmentId)
        {
            var universities = await _universityService.GetAllAsync();
            var departments = new List<DepartmentDTO>();
            var advisors = new List<AdvisorDTO>();

            if (universityId.HasValue)
            {
                departments = (await _departmentService.GetByUniversityIdAsync(universityId.Value)).ToList();
            }

            if (departmentId.HasValue)
            {
                var allAdvisors = await _advisorService.GetAllAsync();
                advisors = allAdvisors.Where(a => a.DepartmentId == departmentId.Value).ToList();
            }

            var viewModel = new HomeFilterViewModel
            {
                SelectedUniversityId = universityId,
                SelectedDepartmentId = departmentId,
                Universities = universities,
                Departments = departments,
                Advisors = advisors
            };

            return View(viewModel);
        }
    }
}
