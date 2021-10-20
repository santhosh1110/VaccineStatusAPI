using Api.Enums;

namespace Api.DTOs
{
    public class EmployeeDto
    {
        public string EmpCode { get; set; }
        public string AadharNumber { get; set; }
        public VaccineStatusEnum VaccineStatus { get; set; }
    }
}
