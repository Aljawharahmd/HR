using HR.Data.Enums;
using System;

namespace HR.Data.DTO.EmployeeLeaves.Parameters
{
    public class EmployeeLeavesCreateParameters
    {
        public int CategoryId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string AttachmentFile { get; set; }
        public LeaveStatus LeaveStatus { get; set; }
    }
}
