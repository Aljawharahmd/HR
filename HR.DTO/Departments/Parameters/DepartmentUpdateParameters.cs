using HR.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.Departments.Parameters
{
    public class DepartmentUpdateParameters
    {
        public string Name { get; set; }
        public int? ManagerId { get; set; }
        public Employee Manager { get; set; }
    }
}
