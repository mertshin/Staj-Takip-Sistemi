using AutoMapper;
using Business.DTOs.DepartmentDtos;
using Business.DTOs.UniversityDtos;
using Business.Interfaces;
using Core.Entities;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUniversityService _universityService;
        private readonly IMapper _mapper;

        public DepartmentService(ApplicationDbContext context,
                               IUniversityService universityService,
                               IMapper mapper)
        {
            _context = context;
            _universityService = universityService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllAsync()
        {
            var departments = await _context.Departments
                .Include(d => d.University)
                .Include(d => d.Students)
                .Include(d => d.Advisors)
                .ToListAsync();

            return _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
        }

        public async Task<DepartmentDTO> GetByIdAsync(int id)
        {
            var department = await _context.Departments
                .Include(d => d.University)
                .Include(d => d.Students)
                .Include(d => d.Advisors)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                return null;

            return _mapper.Map<DepartmentDTO>(department);
        }

        public async Task<IEnumerable<DepartmentDTO>> GetByUniversityIdAsync(int universityId)
        {
            var departments = await _context.Departments
                .Include(d => d.University)
                .Include(d => d.Students)
                .Include(d => d.Advisors)
                .Where(d => d.UniversityId == universityId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
        }

        public async Task<DepartmentDTO> CreateAsync(DepartmentCreateDTO createDto)
        {
            // Kod kontrolü
            if (await CodeExistsAsync(createDto.Code))
            {
                throw new InvalidOperationException("Bu bölüm kodu zaten kullanılmaktadır.");
            }

            // Üniversite kontrolü
            var university = await _universityService.GetByIdAsync(createDto.UniversityId);
            if (university == null)
            {
                throw new InvalidOperationException("Belirtilen üniversite bulunamadı.");
            }

            var department = _mapper.Map<Department>(createDto);

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(department.Id); // ✅ Kendi metodunu çağırıyor
        }

        public async Task<bool> UpdateAsync(int id, DepartmentUpdateDTO updateDto)
        {
            // Varlık kontrolü
            if (!await ExistsAsync(id)) // ✅ Kendi metodunu çağırıyor
                return false;

            var department = await _context.Departments.FindAsync(id);

            // Kod kontrolü (kendi ID'si hariç)
            if (await CodeExistsAsync(updateDto.Code, id)) // ✅ Kendi metodunu çağırıyor
            {
                throw new InvalidOperationException("Bu bölüm kodu zaten kullanılmaktadır.");
            }

            // Üniversite kontrolü
            var university = await _universityService.GetByIdAsync(updateDto.UniversityId);
            if (university == null)
            {
                throw new InvalidOperationException("Belirtilen üniversite bulunamadı.");
            }

            _mapper.Map(updateDto, department);

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Students)
                .Include(d => d.Advisors)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                return false;

            // Bağlı öğrenci veya danışman kontrolü
            if (department.Students?.Any() == true || department.Advisors?.Any() == true)
            {
                throw new InvalidOperationException("Bu bölümde kayıtlı öğrenci veya danışman bulunduğu için silinemez.");
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Departments.AnyAsync(d => d.Id == id);
        }

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
        {
            var query = _context.Departments.Where(d => d.Code == code);

            if (excludeId.HasValue)
            {
                query = query.Where(d => d.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<UniversityDTO>> GetUniversitiesForDropdownAsync()
        {
            return await _universityService.GetAllAsync();
        }
    }
}