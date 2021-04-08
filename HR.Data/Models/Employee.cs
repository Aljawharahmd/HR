using HR.Data.Enums;
using System.Collections.Generic;

namespace HR.Data.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string JobTitle { get; set; }
        public int? DepartmentId { get; set; }

        public Department ManagedDepartment { get; set; }
        public Department Department { get; set; }
        public List<EmployeeLeave> Leaves { get; set; }
    }
}
