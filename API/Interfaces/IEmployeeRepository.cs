using Api.DTOs;
using System.Collections.Generic;

namespace Api.Interfaces
{
    public interface IEmployeeRepository
    {
        void AddEmployeesbyCompanyId(List<EmployeeDto> employees, string companyCode);      
    }
}