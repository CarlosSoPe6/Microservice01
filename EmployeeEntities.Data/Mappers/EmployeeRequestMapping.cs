using AutoMapper;
using EmployeeEntities.Models.Domain;
using EmployeeEntities.Models.Requests;

namespace EmployeeEntities.Data.Mappers;

public class EmployeeRequestMapping : Profile
{
    public EmployeeRequestMapping()
    {
        CreateMap<EmployeeRequest, Employee>()
            .ForMember(
                dest => dest.name,
                opt => opt.MapFrom(src => src.name)
            )
            .ForMember(
                dest => dest.lastName,
                opt => opt.MapFrom(src => src.lastName)
            )
            .ForMember(
                dest => dest.region,
                opt => opt.MapFrom(src => src.region)
            );
    }
}
