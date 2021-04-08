using HR.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.Employees.Results
{
    public class EmployeeViewResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string DepartmentName { get; set; }
        public string ManagerName { get; set; }
    }
}
