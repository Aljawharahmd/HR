using HR.Data.DTO.Enums;

namespace HR.Data.DTO.Departments.Results
{
    public class DepartmentUpdateResult
    {
        public DepartmentUpdateResult()
        {

        }
        public DepartmentUpdateResult(DepartmentValidationStatus status)
        {
            Status = status;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ManagerName { get; set; }
        public DepartmentValidationStatus Status { get; set; }
    }
}
