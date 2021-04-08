namespace HR.Data.DTO.Employees
{
    public class EmployeeSearchParametrs
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }

        public int? DepartmentId { get; set; }
        public int? ManagerId { get; set; }
    }
}
