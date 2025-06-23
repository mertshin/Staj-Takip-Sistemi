using AutoMapper;     // DTO'ların namespace'i
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
        }
    }
}
