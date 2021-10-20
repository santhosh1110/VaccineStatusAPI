using Api.DTOs;
using Api.Entities;
using AutoMapper;
using System.Linq;

namespace Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(x => x.Employees, opt => opt.MapFrom(src => src.Employees.ToList()));
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<CompanyInsertDto, Company>();
            CreateMap<CompanyDto, Company>();

        }
    }
}