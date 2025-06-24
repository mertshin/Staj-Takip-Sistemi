using AutoMapper;
using Business.DTOs.AdvisorDtos;
using Business.Interfaces;
using Core.Entities;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class AdvisorService : IAdvisorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AdvisorService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AdvisorDTO>> GetAllAsync()
        {
            var advisors = await _context.Advisors
                .Include(a => a.Department)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AdvisorDTO>>(advisors);
        }

        public async Task<AdvisorDTO> GetByIdAsync(int id)
        {
            var advisor = await _context.Advisors
                .Include(a => a.Department)
                .FirstOrDefaultAsync(a => a.Id == id);

            return advisor == null ? null : _mapper.Map<AdvisorDTO>(advisor);
        }

        public async Task<AdvisorDTO> CreateAsync(AdvisorCreateDTO createDto)
        {
            var advisor = _mapper.Map<Advisor>(createDto);
            _context.Advisors.Add(advisor);
            await _context.SaveChangesAsync();
            return _mapper.Map<AdvisorDTO>(advisor);
        }

        public async Task<bool> UpdateAsync(int id, AdvisorUpdateDTO updateDto)
        {
            var advisor = await _context.Advisors.FindAsync(id);
            if (advisor == null) return false;

            _mapper.Map(updateDto, advisor);
            _context.Advisors.Update(advisor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var advisor = await _context.Advisors.FindAsync(id);
            if (advisor == null) return false;

            _context.Advisors.Remove(advisor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Advisors.AnyAsync(a => a.Id == id);
        }
    }
}
