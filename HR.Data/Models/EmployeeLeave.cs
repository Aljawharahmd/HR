using HR.Data.Enums;
using System;

namespace HR.Data.Models
{
    public class EmployeeLeave
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; }
        public int Duration { get; set; }
        public string AttachmentFile { get; set; }
        public string RejectReason { get; set; }


        public int CategoryId { get; set; }
        public int EmployeeId { get; set; }
        public LeaveCategory LeaveCategory { get; set; }
        public Employee Employee { get; set; }
    }
}
