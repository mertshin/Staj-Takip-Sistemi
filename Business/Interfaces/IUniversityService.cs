using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.DTOs.UniversityDtos;

namespace Business.Interfaces
{
    public interface IUniversityService
    {
        Task<IEnumerable<UniversityDTO>> GetAllAsync();
        Task<UniversityDTO> GetByIdAsync(int id);
        Task<UniversityDTO> CreateAsync(UniversityCreateDTO createDto);
        Task<bool> UpdateAsync(int id, UniversityUpdateDTO updateDto);
        Task<bool> DeleteAsync(int id);
    }
}
