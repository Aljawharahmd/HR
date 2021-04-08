using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.Employees.Parameters
{
    public class EmployeeCreateParameters
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string JobTitle { get; set; }
        public int DepartmentId { get; set; }
    }
}
