using AutoMapper;     // DTO'ların namespace'i
using Business.DTOs.AdvisorDtos;
using Business.DTOs.DepartmentDtos;
using Business.DTOs.UniversityDtos;
using Business.DTOs.StudentDtos;
using Business.DTOs.InternshipDiaryDtos;
using Core.Entities;    // Entitylerin namespace'i

namespace Business.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<University, UniversityDTO>().ReverseMap();
            CreateMap<UniversityCreateDTO, University>();
            CreateMap<UniversityUpdateDTO, University>();

            CreateMap<Department, DepartmentDTO>()
    .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.University.Name))
    .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Students != null ? src.Students.Count : 0))
    .ForMember(dest => dest.AdvisorCount, opt => opt.MapFrom(src => src.Advisors != null ? src.Advisors.Count : 0));

            CreateMap<DepartmentCreateDTO, Department>();
            CreateMap<DepartmentUpdateDTO, Department>();
            CreateMap<DepartmentDTO, DepartmentUpdateDTO>();

            CreateMap<Advisor, AdvisorDTO>()
    .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<AdvisorCreateDTO, Advisor>();
            CreateMap<AdvisorUpdateDTO, Advisor>();

            // Student mappings
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => src.Department.Code))
                .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.Department.University.Name))
                .ForMember(dest => dest.AdvisorName, opt => opt.MapFrom(src => $"{src.Advisor.FirstName} {src.Advisor.LastName}"))
                .ForMember(dest => dest.InternshipApplicationCount, opt => opt.MapFrom(src => src.InternshipApplications != null ? src.InternshipApplications.Count : 0))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            CreateMap<StudentCreateDTO, Student>();
            CreateMap<StudentUpdateDTO, Student>();

            // InternshipDiary mappings
            CreateMap<InternshipDiary, InternshipDiaryDTO>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.InternshipApplication.Student.FirstName} {src.InternshipApplication.Student.LastName}"))
                .ForMember(dest => dest.StudentNumber, opt => opt.MapFrom(src => src.InternshipApplication.Student.StudentNumber))
                .ForMember(dest => dest.InternshipTopic, opt => opt.MapFrom(src => src.InternshipApplication.InternshipTopic))
                .ForMember(dest => dest.ApprovedByAdvisorName, opt => opt.MapFrom(src => src.ApprovedByAdvisor != null ? $"{src.ApprovedByAdvisor.FirstName} {src.ApprovedByAdvisor.LastName}" : null));

            CreateMap<InternshipDiaryCreateDTO, InternshipDiary>();
        }
    }
}
