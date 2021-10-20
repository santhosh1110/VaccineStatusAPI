using Api.DTOs;
using Api.Enums;
using Api.Entities;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CompaniesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        public CompaniesController (ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<CompanyDto>> GetAllCompanies()
        {
            return await _companyRepository.GetAllCompaiesAsync();
        }

        [HttpGet]
        public async Task<ActionResult<CompanyDto>> GetCompany(string companyCode)
        {
            var company = await _companyRepository.GetCompanybyCodeAsync(companyCode);
            return company;
        }

        [HttpGet]
        public void LoadCompanies()
        {
            var companyData = System.IO.File.ReadAllText("Data/CompanyData.json");
            var companies = JsonSerializer.Deserialize<List<Company>>(companyData);
            if (companies == null) 
            {
                return;
            }
            _companyRepository.CreateCompanyFromSeedData(companies);
        }

        [HttpGet]
        public async Task<IEnumerable<CompanyDto>> SearchCompany(string companyName)
        {
            return  await _companyRepository.GetCompaiesyByNameAsync(companyName);
        }

        [HttpPost]
        public async Task<bool> UpdateEmployeeVaccineStatus(string employeecode, VaccineStatusEnum vaccinestatus)
        {
            return await _companyRepository.UpdateEmployeeVaccineStatus(employeecode, vaccinestatus);
        }

        [HttpPut]
        public async Task<ActionResult> InsertorUpdateCompany(CompanyInsertDto companyDto)
        {
            bool result=  await _companyRepository.InsertorUpdateCompany(companyDto);
            if (result) return Ok();
            else return BadRequest("No Such company");
        }

        [HttpGet]
        public async Task<bool> CheckCompanyReadytoOpen(string companyCode)
        {
           return await _companyRepository.CheckCompanyReadytoOpen(companyCode);
        }

    }
}