using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities
{
    public class Company
    {
        [Key]
        [StringLength(8, MinimumLength = 3)]
        [Required]
        public string CompanyCode { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required] 
        public string CompanyName { get; set; }
        public ICollection<Employee> Employees { get; set; }

    }
}