using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTOs.UniversityDtos;
using Business.Interfaces;
using Core.Entities;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class UniversityService : IUniversityService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UniversityService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UniversityDTO>> GetAllAsync()
        {
            var universities = await _context.Universities.ToListAsync();
            return _mapper.Map<List<UniversityDTO>>(universities);
        }

        public async Task<UniversityDTO> GetByIdAsync(int id)
        {
            var university = await _context.Universities.FindAsync(id);
            if (university == null) return null;
            return _mapper.Map<UniversityDTO>(university);
        }

        public async Task<UniversityDTO> CreateAsync(UniversityCreateDTO createDto)
        {
            var university = _mapper.Map<University>(createDto);
            _context.Universities.Add(university);
            await _context.SaveChangesAsync();
            return _mapper.Map<UniversityDTO>(university);
        }

        public async Task<bool> UpdateAsync(int id, UniversityUpdateDTO updateDto)
        {
            var university = await _context.Universities.FindAsync(id);
            if (university == null) return false;

            _mapper.Map(updateDto, university);
            _context.Universities.Update(university);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var university = await _context.Universities.FindAsync(id);
            if (university == null) return false;

            _context.Universities.Remove(university);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
