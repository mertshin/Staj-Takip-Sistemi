using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.DTOs.DepartmentDtos;
using Business.DTOs.UniversityDtos;

namespace Business.Interfaces
{
    public interface  IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllAsync();
        Task<DepartmentDTO> GetByIdAsync(int id);
        Task<IEnumerable<DepartmentDTO>> GetByUniversityIdAsync(int universityId);
        Task<DepartmentDTO> CreateAsync(DepartmentCreateDTO createDto);
        Task<bool> UpdateAsync(int id, DepartmentUpdateDTO updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code, int? excludeId = null); //ExcludeId güncelleme yaptıgında aynısıysa engeller ..
        Task<IEnumerable<UniversityDTO>> GetUniversitiesForDropdownAsync();
    }
}
