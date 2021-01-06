using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {
        [Key]
        // Not to be confused with Employee Id. This is unique for this Model
        public string EmployeeId { get; set; }

        public decimal Salary { get; set; }

        public DateTime EffectiveDate { get; set; }
    }
}
