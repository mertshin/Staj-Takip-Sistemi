using Business.DTOs.AdvisorDtos;

namespace Business.Interfaces
{
    public interface IAdvisorService
    {
        Task<IEnumerable<AdvisorDTO>> GetAllAsync();
        Task<AdvisorDTO> GetByIdAsync(int id);
        Task<AdvisorDTO> CreateAsync(AdvisorCreateDTO createDto);
        Task<bool> UpdateAsync(int id, AdvisorUpdateDTO updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}