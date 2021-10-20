using Api.Controllers;
using Api.DTOs;
using Api.Enums;
using Api.Data;
using Api.Helpers;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;


namespace Api.Tests
{
    public class CompaniesControllerTests
    {
        private static IMapper _mapper;
        private static CompaniesController companiesController;
       
        public CompaniesControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfiles());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;

                //Arrange
                companiesController = ArrangeController();
            }
        }

        [Fact]
        public async Task GetCompanies_WhenCalled_ReturnsCompaniesList()
        {
            // Act
            var result = await companiesController.GetAllCompanies();

            // Assert
            Assert.True(result.Count>0);
            Assert.Equal(2, result.Count);
            Assert.Equal("SEQAT", result[0].CompanyCode);
            Assert.Equal("WIPRO", result[1].CompanyCode);
        }

        [Theory]
        [InlineData("SEQAT", true)]
        [InlineData("SEQL", false)]
        public async Task GetCompany_WhenCalled_ReturnsCompany(string companycode, bool resultExpected)
        {
            // Act
            var result = await companiesController.GetCompany(companycode);

            // Assert
            Assert.Equal(resultExpected, companycode==(result.Value==null?null:result.Value.CompanyCode));
        }

        [Theory]
        [InlineData("IN0045", VaccineStatusEnum.IstDose,  true)]
        [InlineData("IN0015", VaccineStatusEnum.IIndDose,  true)]
        public async Task UpdateEmployeeStatus_WhenCalled_ReturnsBoolean(string empcode, VaccineStatusEnum status, bool resultExpected)
        {
            // Act            
            var result = await companiesController.UpdateEmployeeVaccineStatus(empcode, status);

            // Assert
            Assert.Equal(resultExpected, result);
        }

        [Theory]
        [ClassData(typeof(testCompanyReady))]
        public async Task InsertCompanyTest_WhenCalled_ReturnsOKResult(CompanyInsertDto company) 
        {
            // Act
            var result = await companiesController.InsertorUpdateCompany(company);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Theory]
        [InlineData("SEQAT", true)]
        public async Task TestCompanyReadytoOpen_WhenCalled_ReturnsCompanyDetails(string companyCode, bool resultExpected)
        {
            // Act
            var result = await companiesController.CheckCompanyReadytoOpen(companyCode);

            // Assert
            // if all the employees completed at least first dose company will be ready to open
            Assert.Equal(resultExpected, result);

        }

         [Theory]
        [ClassData(typeof(testCompanyReady))]
        public async Task TestCompanyReadyAfterInsertandUpdatevaccineStatus_WhenCalled_ReturnsCompanyDetails(CompanyInsertDto company)
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<VaccineContext>();
            builder.UseInMemoryDatabase("test.db");
            var options = builder.Options;
            var vaccineContext = new VaccineContext(options);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfiles());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var companyRepository = new CompanyRepository(vaccineContext, mapper);
            var companiesController = new CompaniesController(companyRepository, mapper);

            //Arrange
            CompanyInsertDto companyNull = null;
            // Act
            var result = await companiesController.InsertorUpdateCompany(companyNull);
            // Assert
            // Test when Company data is null  
            Assert.IsType<BadRequestObjectResult>(result);


            var resultInsert = await companiesController.InsertorUpdateCompany(company);
            // Assert
            //Test whether the data is inserted.
            Assert.IsType<OkResult>(resultInsert);

            // Act
            var checkReadytoOpen = await companiesController.CheckCompanyReadytoOpen(company.CompanyCode);
            // Assert
            // if all the employees completed at least first dose company will be ready to open
            Assert.False(checkReadytoOpen);

            // Act            
            var resultUpdate = await companiesController.UpdateEmployeeVaccineStatus("TS21523", VaccineStatusEnum.IstDose) ;
            // Assert
            // assert update status
            Assert.True(resultUpdate);

            // Act
            checkReadytoOpen = await companiesController.CheckCompanyReadytoOpen(company.CompanyCode);
            // Assert
            // if all the employees completed at least first dose company will be ready to open
            Assert.True(checkReadytoOpen);
            
        }

        #region Private Methods
        private static CompaniesController ArrangeController()
        {
            var mockCompanyRepository = new Mock<ICompanyRepository>();
            mockCompanyRepository.Setup(x => x.GetAllCompaiesAsync()).ReturnsAsync(CompaniesList());
            mockCompanyRepository.Setup(x => x.CheckCompanyReadytoOpen(It.IsAny<string>())).ReturnsAsync(true);
            mockCompanyRepository.Setup(x => x.GetCompaiesyByNameAsync(It.IsAny<string>())).ReturnsAsync(CompaniesList());
            mockCompanyRepository.Setup(x => x.GetCompanybyCodeAsync(It.IsAny<string>())).ReturnsAsync(CompaniesList()[0]);
            mockCompanyRepository.Setup(x => x.InsertorUpdateCompany(It.IsAny<CompanyInsertDto>())).ReturnsAsync(true);
            mockCompanyRepository.Setup(x => x.UpdateEmployeeVaccineStatus(It.IsAny<string>(), It.IsAny<VaccineStatusEnum>())).ReturnsAsync(true);

            return new CompaniesController(mockCompanyRepository.Object, _mapper);
        }

        public static List<CompanyDto> CompaniesList()
        {
            var companyData = System.IO.File.ReadAllText("Data/CompanySeedData.json");
            var companies = JsonSerializer.Deserialize<List<CompanyDto>>(companyData);
            return companies;
        }

        private class testCompanyReady : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {
                        new CompanyInsertDto {
                            CompanyCode = "TCS",
                            CompanyName = "Tata Consultancy Services",
                            Employees = new List<EmployeeDto>()
                            {
                                new EmployeeDto()
                                {
                                    AadharNumber="803366558082",
                                    EmpCode="TS21523",
                                    VaccineStatus=Enums.VaccineStatusEnum.None
                                },
                                new EmployeeDto()
                                {
                                    AadharNumber="803366558082",
                                    EmpCode="TS21423",
                                    VaccineStatus=Enums.VaccineStatusEnum.IIndDose
                                }
                            }
                        }
                    };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


        #endregion
    }
   
}
