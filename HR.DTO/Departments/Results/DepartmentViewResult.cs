using HR.Data.DTO.Employees.Results;
using HR.Data.Models;
using System.Collections.Generic;

namespace HR.Data.DTO.Departments.Results
{
    public class DepartmentViewResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ManagerName { get; set; }
        public List<EmployeeViewResult> Employees { get; set; }
    }
}
