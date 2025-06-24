using Business.DTOs.InternshipDiaryDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IInternshipDiaryService
    {
        // CRUD Operations
        Task<IEnumerable<InternshipDiaryDTO>> GetAllAsync();
        Task<InternshipDiaryDTO?> GetByIdAsync(int id);
        Task<IEnumerable<InternshipDiaryDTO>> GetByInternshipApplicationIdAsync(int applicationId);
        Task<IEnumerable<InternshipDiaryDTO>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<InternshipDiaryDTO>> GetByAdvisorIdForApprovalAsync(int advisorId);
        Task<InternshipDiaryDTO> CreateAsync(InternshipDiaryCreateDTO createDto);
        Task<bool> UpdateAsync(int id, InternshipDiaryCreateDTO updateDto);
        Task<bool> DeleteAsync(int id);
        
        // Approval Operations
        Task<bool> ApproveAsync(InternshipDiaryApprovalDTO approvalDto);
        Task<bool> RejectAsync(InternshipDiaryApprovalDTO approvalDto);
        
        // Statistics
        Task<int> GetPendingCountByAdvisorAsync(int advisorId);
        Task<int> GetApprovedCountByStudentAsync(int studentId);
    }
} 