using Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities
{
    public class Employee
    {
        [Key]
        [StringLength(10)]
        [Required]
        public string EmpCode { get; set; }
        [Required]
        [StringLength(14)]
        [RegularExpression(@"^(\d{14}|\d{14})$", ErrorMessage = "Enter Valid Aadhar Number")]
        public string AadharNumber { get; set; }
        [Required]
        public VaccineStatusEnum VaccineStatus { get; set; }
        public Company company { get; set; }
        

    }
}
