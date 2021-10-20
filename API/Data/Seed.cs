using Api.Entities;
using Api.Interfaces;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Api.Data
{
    public class Seed
    {
        //Read Company data from Json File and update Database
        public static void SeedCompanies(ICompanyRepository companyRepository, ILogger logger)
        {
            if (companyRepository.GetAllCompaiesAsync().Result.Count>0) return;
            var companyData = System.IO.File.ReadAllText("Data/CompanySeedData.json");
            var companies = JsonSerializer.Deserialize<List<Company>>(companyData);
            if (companies == null) 
            {
                logger.LogError("Seed companies No company found");
                return;
                
            }

            companyRepository.CreateCompanyFromSeedData(companies);
        }
    }
}
