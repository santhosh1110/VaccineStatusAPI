using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class CompanyDto :CompanyInsertDto
    {
        public int EmployeesCount 
        {
            get
            {
                return Employees.Count;
            }
        }
        public int IstDoseCompleted 
        {
            get
            {
                return Employees.FindAll(c=>c.VaccineStatus==Enums.VaccineStatusEnum.IstDose).Count;
            }
        }
        public int IIndDoseCompleted
        {
            get
            {
                return Employees.FindAll(c => c.VaccineStatus == Enums.VaccineStatusEnum.IIndDose).Count;
            }
        }
        public bool ReadyToOpen { 
            get
            {
                return EmployeesCount>0 && EmployeesCount == IstDoseCompleted + IIndDoseCompleted;
            }
        }
    }
    public class CompanyInsertDto
    {
        [Required]
        public string CompanyCode { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public List<EmployeeDto> Employees { get; set; }
       
    }
}