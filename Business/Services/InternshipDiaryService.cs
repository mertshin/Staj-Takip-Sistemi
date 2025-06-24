using AutoMapper;
using Business.DTOs.InternshipDiaryDtos;
using Business.Interfaces;
using Core.Entities;
using Core.Enums;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class InternshipDiaryService : IInternshipDiaryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public InternshipDiaryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InternshipDiaryDTO>> GetAllAsync()
        {
            var diaries = await _context.InternshipDiaries
                .Include(d => d.InternshipApplication)
                .ThenInclude(ia => ia.Student)
                .Include(d => d.ApprovedByAdvisor)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InternshipDiaryDTO>>(diaries);
        }

        public async Task<InternshipDiaryDTO?> GetByIdAsync(int id)
        {
            var diary = await _context.InternshipDiaries
                .Include(d => d.InternshipApplication)
                .ThenInclude(ia => ia.Student)
                .Include(d => d.ApprovedByAdvisor)
                .FirstOrDefaultAsync(d => d.Id == id);

            return diary != null ? _mapper.Map<InternshipDiaryDTO>(diary) : null;
        }

        public async Task<IEnumerable<InternshipDiaryDTO>> GetByInternshipApplicationIdAsync(int applicationId)
        {
            var diaries = await _context.InternshipDiaries
                .Include(d => d.InternshipApplication)
                .ThenInclude(ia => ia.Student)
                .Include(d => d.ApprovedByAdvisor)
                .Where(d => d.InternshipApplicationId == applicationId)
                .OrderByDescending(d => d.Date)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InternshipDiaryDTO>>(diaries);
        }

        public async Task<IEnumerable<InternshipDiaryDTO>> GetByStudentIdAsync(int studentId)
        {
            var diaries = await _context.InternshipDiaries
                .Include(d => d.InternshipApplication)
                .ThenInclude(ia => ia.Student)
                .Include(d => d.ApprovedByAdvisor)
                .Where(d => d.InternshipApplication.StudentId == studentId)
                .OrderByDescending(d => d.Date)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InternshipDiaryDTO>>(diaries);
        }

        public async Task<IEnumerable<InternshipDiaryDTO>> GetByAdvisorIdForApprovalAsync(int advisorId)
        {
            var diaries = await _context.InternshipDiaries
                .Include(d => d.InternshipApplication)
                .ThenInclude(ia => ia.Student)
                .Include(d => d.ApprovedByAdvisor)
                .Where(d => d.InternshipApplication.AdvisorId == advisorId && 
                           d.ApprovalStatus == InternshipStatus.Pending)
                .OrderBy(d => d.Date)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InternshipDiaryDTO>>(diaries);
        }

        public async Task<InternshipDiaryDTO> CreateAsync(InternshipDiaryCreateDTO createDto)
        {
            // Aynı tarihte günlük var mı kontrol et
            var existingDiary = await _context.InternshipDiaries
                .FirstOrDefaultAsync(d => d.InternshipApplicationId == createDto.InternshipApplicationId && 
                                         d.Date.Date == createDto.Date.Date);

            if (existingDiary != null)
                throw new InvalidOperationException("Bu tarih için zaten günlük kaydı mevcut");

            var diary = _mapper.Map<InternshipDiary>(createDto);
            diary.ApprovalStatus = InternshipStatus.Pending;

            _context.InternshipDiaries.Add(diary);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(diary.Id) ?? throw new InvalidOperationException("Günlük oluşturulamadı");
        }

        public async Task<bool> UpdateAsync(int id, InternshipDiaryCreateDTO updateDto)
        {
            var diary = await _context.InternshipDiaries.FindAsync(id);
            if (diary == null) return false;

            // Onaylanmış günlükler düzenlenemez
            if (diary.ApprovalStatus == InternshipStatus.Approved)
                throw new InvalidOperationException("Onaylanmış günlükler düzenlenemez");

            // Aynı tarihte başka günlük var mı kontrol et (mevcut hariç)
            var existingDiary = await _context.InternshipDiaries
                .FirstOrDefaultAsync(d => d.Id != id &&
                                         d.InternshipApplicationId == updateDto.InternshipApplicationId && 
                                         d.Date.Date == updateDto.Date.Date);

            if (existingDiary != null)
                throw new InvalidOperationException("Bu tarih için zaten günlük kaydı mevcut");

            _mapper.Map(updateDto, diary);
            diary.ApprovalStatus = InternshipStatus.Pending; // Düzenleme sonrası tekrar onay bekler
            diary.ApprovedDate = null;
            diary.ApprovedByAdvisorId = null;
            diary.RejectionReason = null;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var diary = await _context.InternshipDiaries.FindAsync(id);
            if (diary == null) return false;

            // Onaylanmış günlükler silinemez
            if (diary.ApprovalStatus == InternshipStatus.Approved)
                throw new InvalidOperationException("Onaylanmış günlükler silinemez");

            _context.InternshipDiaries.Remove(diary);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveAsync(InternshipDiaryApprovalDTO approvalDto)
        {
            var diary = await _context.InternshipDiaries.FindAsync(approvalDto.DiaryId);
            if (diary == null) return false;

            if (diary.ApprovalStatus != InternshipStatus.Pending)
                throw new InvalidOperationException("Bu günlük zaten değerlendirilmiş");

            diary.ApprovalStatus = InternshipStatus.Approved;
            diary.ApprovedDate = DateTime.Now;
            diary.ApprovedByAdvisorId = approvalDto.AdvisorId;
            diary.RejectionReason = null;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectAsync(InternshipDiaryApprovalDTO approvalDto)
        {
            var diary = await _context.InternshipDiaries.FindAsync(approvalDto.DiaryId);
            if (diary == null) return false;

            if (diary.ApprovalStatus != InternshipStatus.Pending)
                throw new InvalidOperationException("Bu günlük zaten değerlendirilmiş");

            diary.ApprovalStatus = InternshipStatus.Rejected;
            diary.ApprovedDate = DateTime.Now;
            diary.ApprovedByAdvisorId = approvalDto.AdvisorId;
            diary.RejectionReason = approvalDto.RejectionReason;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetPendingCountByAdvisorAsync(int advisorId)
        {
            return await _context.InternshipDiaries
                .CountAsync(d => d.InternshipApplication.AdvisorId == advisorId && 
                                d.ApprovalStatus == InternshipStatus.Pending);
        }

        public async Task<int> GetApprovedCountByStudentAsync(int studentId)
        {
            return await _context.InternshipDiaries
                .CountAsync(d => d.InternshipApplication.StudentId == studentId && 
                                d.ApprovalStatus == InternshipStatus.Approved);
        }
    }
} 