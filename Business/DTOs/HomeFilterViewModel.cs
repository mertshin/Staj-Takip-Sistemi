using Business.DTOs.AdvisorDtos;
using Business.DTOs.DepartmentDtos;
using Business.DTOs.UniversityDtos;

namespace Web.Models.ViewModels
{
    public class HomeFilterViewModel
    {
        public int? SelectedUniversityId { get; set; }
        public int? SelectedDepartmentId { get; set; }

        public IEnumerable<UniversityDTO> Universities { get; set; }
        public IEnumerable<DepartmentDTO> Departments { get; set; }
        public IEnumerable<AdvisorDTO> Advisors { get; set; }
    }
}
