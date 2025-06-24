using Business.DTOs.StudentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllAsync();
        Task<StudentDTO?> GetByIdAsync(int id);
        Task<IEnumerable<StudentDTO>> GetByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<StudentDTO>> GetByAdvisorIdAsync(int advisorId);
        Task<StudentDTO> CreateAsync(StudentCreateDTO createDto);
        Task<bool> UpdateAsync(int id, StudentUpdateDTO updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> StudentNumberExistsAsync(string studentNumber, int? excludeId = null);
        Task<bool> IdentityNumberExistsAsync(string identityNumber, int? excludeId = null);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);

        // Dropdown için
        Task<IEnumerable<dynamic>> GetDepartmentsForDropdownAsync();
        Task<IEnumerable<dynamic>> GetAdvisorsByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<dynamic>> GetUniversitiesForDropdownAsync();
    }
}