using AutoMapper;     // DTO'ların namespace'i
using Business.DTOs.DepartmentDtos;
using Business.DTOs.UniversityDtos;
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
        }
    }
}
