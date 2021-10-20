using Api.DTOs;
using Api.Entities;
using Api.Enums;
using Api.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly VaccineContext _context;
        private readonly IMapper _mapper;
        public CompanyRepository(VaccineContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<CompanyDto>> GetCompaiesyByNameAsync(string companyName)
        {
            return await _context.Companies
                 .Where(x => x.CompanyName.Contains(companyName))
                 .ProjectTo<CompanyDto>(_mapper.ConfigurationProvider)
                 .ToListAsync();
        }

        public async Task<CompanyDto> GetCompanybyCodeAsync(string companyCode)
        {
            return await _context.Companies
                   .Where(x => x.CompanyCode == companyCode)
                   .ProjectTo<CompanyDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync();
        }

        public async Task<List<CompanyDto>> GetAllCompaiesAsync()
        {
            return await _context.Companies
                   .ProjectTo<CompanyDto>(_mapper.ConfigurationProvider)
                   .ToListAsync();
        }

        public async Task<bool> CheckCompanyReadytoOpen(string companyCode)
        {
            var company= await _context.Companies
                   .Where(x => x.CompanyCode == companyCode)
                   .ProjectTo<CompanyDto>(_mapper.ConfigurationProvider)
                   .SingleOrDefaultAsync();

            return company==null?false: company.ReadyToOpen;
        }

        public async Task<bool> InsertorUpdateCompany(CompanyInsertDto company)
        {
            if (company == null) return false;

            Company com = await _context.Companies
                    .Where(x => x.CompanyCode == company.CompanyCode)
                    .SingleOrDefaultAsync();
            if(com==null)
            {
                Company cominSert = new Company();
                _mapper.Map(company, cominSert); 
                _mapper.Map(company.Employees, cominSert.Employees);
                _context.Companies.Add(cominSert);
            }
            else
            {
                com.CompanyName = company.CompanyName;
                com.CompanyCode = company.CompanyCode;
                List<Employee> employees = new List<Employee>();
                _mapper.Map(company.Employees, employees);
                com.Employees = new List<Employee>();
                foreach (Employee emp in employees)
                    com.Employees.Add(emp);
            }

            if (_context.SaveChanges() > 0) return true;
            else return false;            
        }

        public void CreateCompanyFromSeedData(List<Company> companies) 
        {
            _context.Companies.AddRangeAsync(companies);
            _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateEmployeeVaccineStatus(string employeecode, VaccineStatusEnum vaccinestatus)
        {
            Employee employee =  await _context.Employees
                          .Where(x => x.EmpCode == employeecode)
                          .SingleAsync();
            if (employee == null) return false;
            employee.VaccineStatus = vaccinestatus;
            if ( _context.SaveChanges() > 0) return true;
            else return false;
        }
    }
}