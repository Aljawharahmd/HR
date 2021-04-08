using HR.Data.Enums;
using System;

namespace HR.Data.DTO.EmployeeLeaves.Parameters
{
    public class EmployeeLeavesSearchParameters
    {
        public int? CategoryId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LeaveStatus? Status { get; set; }
    }
}
