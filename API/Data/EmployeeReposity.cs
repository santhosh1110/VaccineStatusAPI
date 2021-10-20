using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;

namespace Api.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly VaccineContext _context;
        private readonly IMapper _mapper;
        public EmployeeRepository(VaccineContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddEmployeesbyCompanyId(List<EmployeeDto> employees, string companyCode)
        {
            Company com = _context.Companies
                    .Where(x => x.CompanyCode == companyCode)
                    .SingleOrDefault();
            com.Employees = employees.AsQueryable<EmployeeDto>()
                            .ProjectTo<Employee>(_mapper.ConfigurationProvider).ToList();
            _context.SaveChanges();
        }
    }
}