using AutoMapper;
using Business.DTOs.StudentDtos;
using Business.Interfaces;
using Core.Entities;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StudentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllAsync()
        {
            var students = await _context.Students
                .Include(s => s.Department)
                .ThenInclude(d => d.University)
                .Include(s => s.Advisor)
                .ToListAsync();

            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<StudentDTO?> GetByIdAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.Department)
                .ThenInclude(d => d.University)
                .Include(s => s.Advisor)
                .FirstOrDefaultAsync(s => s.Id == id);

            return student != null ? _mapper.Map<StudentDTO>(student) : null;
        }

        public async Task<IEnumerable<StudentDTO>> GetByDepartmentIdAsync(int departmentId)
        {
            var students = await _context.Students
                .Include(s => s.Department)
                .ThenInclude(d => d.University)
                .Include(s => s.Advisor)
                .Where(s => s.DepartmentId == departmentId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<IEnumerable<StudentDTO>> GetByAdvisorIdAsync(int advisorId)
        {
            var students = await _context.Students
                .Include(s => s.Department)
                .ThenInclude(d => d.University)
                .Include(s => s.Advisor)
                .Where(s => s.AdvisorId == advisorId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<StudentDTO> CreateAsync(StudentCreateDTO createDto)
        {
            // Validation checks
            if (await StudentNumberExistsAsync(createDto.StudentNumber))
                throw new InvalidOperationException("Bu öğrenci numarası zaten kullanımda");

            if (await IdentityNumberExistsAsync(createDto.IdentityNumber))
                throw new InvalidOperationException("Bu kimlik numarası zaten kayıtlı");

            if (await EmailExistsAsync(createDto.Email))
                throw new InvalidOperationException("Bu e-posta adresi zaten kullanımda");

            var student = _mapper.Map<Student>(createDto);
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(student.Id) ?? throw new InvalidOperationException("Öğrenci oluşturulamadı");
        }

        public async Task<bool> UpdateAsync(int id, StudentUpdateDTO updateDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            // Validation checks (excluding current student)
            if (await StudentNumberExistsAsync(updateDto.StudentNumber, id))
                throw new InvalidOperationException("Bu öğrenci numarası zaten kullanımda");

            if (await IdentityNumberExistsAsync(updateDto.IdentityNumber, id))
                throw new InvalidOperationException("Bu kimlik numarası zaten kayıtlı");

            if (await EmailExistsAsync(updateDto.Email, id))
                throw new InvalidOperationException("Bu e-posta adresi zaten kullanımda");

            _mapper.Map(updateDto, student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StudentNumberExistsAsync(string studentNumber, int? excludeId = null)
        {
            return await _context.Students
                .Where(s => excludeId == null || s.Id != excludeId)
                .AnyAsync(s => s.StudentNumber == studentNumber);
        }

        public async Task<bool> IdentityNumberExistsAsync(string identityNumber, int? excludeId = null)
        {
            return await _context.Students
                .Where(s => excludeId == null || s.Id != excludeId)
                .AnyAsync(s => s.IdentityNumber == identityNumber);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await _context.Students
                .Where(s => excludeId == null || s.Id != excludeId)
                .AnyAsync(s => s.Email == email);
        }

        public async Task<IEnumerable<dynamic>> GetDepartmentsForDropdownAsync()
        {
            return await _context.Departments
                .Include(d => d.University)
                .Select(d => new { d.Id, Name = $"{d.Name} ({d.University.Name})" })
                .ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAdvisorsByDepartmentIdAsync(int departmentId)
        {
            return await _context.Advisors
                .Where(a => a.DepartmentId == departmentId)
                .Select(a => new { a.Id, Name = $"{a.Title} {a.FirstName} {a.LastName}" })
                .ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetUniversitiesForDropdownAsync()
        {
            return await _context.Universities
                .Select(u => new { u.Id, u.Name })
                .ToListAsync();
        }
    }
} 