using HR.Data.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.Departments.Results
{
    public class DepartmentCreateResult
    {
        public DepartmentViewResult Data { get; set; }
        public DepartmentValidationStatus Status { get; set; }
    }
}
