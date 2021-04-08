using HR.Data.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.Employees.Results
{
    public class EmployeeUpdateResult
    {
        public EmployeeViewResult Data { get; set; }
        public EmployeeValidationStatus Status { get; set; }
    }
}
