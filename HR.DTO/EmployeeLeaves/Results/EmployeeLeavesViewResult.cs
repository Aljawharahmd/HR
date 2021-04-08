using HR.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.EmployeeLeaves.Results
{
    public class EmployeeLeavesViewResult
    {
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string AttachmentFile { get; set; }
        public LeaveStatus Status { get; set; }
        public string RejectReason { get; set; }
    }
}
