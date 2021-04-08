using HR.Data.DTO.Enums;

namespace HR.Data.DTO.Employees.Results
{
    public class EmployeeCreateResult
    {
        public EmployeeCreateResult()
        {
        }
        public EmployeeCreateResult(EmployeeValidationStatus status)
        {
            Status = status;
        }

        public EmployeeViewResult Data { get; set; }
        public EmployeeValidationStatus Status { get; set; }
    }
}
