using Api.DTOs;
using Api.Entities;
using Api.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface ICompanyRepository
    {
        void CreateCompanyFromSeedData(List<Company> companies);
        Task<bool> InsertorUpdateCompany(CompanyInsertDto company);
        Task<List<CompanyDto>> GetAllCompaiesAsync();
        Task<CompanyDto> GetCompanybyCodeAsync(string companyCode);
        Task<List<CompanyDto>> GetCompaiesyByNameAsync(string companyName);
        Task<bool> CheckCompanyReadytoOpen(string companyCode);
        Task<bool> UpdateEmployeeVaccineStatus(string employeecode, VaccineStatusEnum vaccinestatus);
    }
}