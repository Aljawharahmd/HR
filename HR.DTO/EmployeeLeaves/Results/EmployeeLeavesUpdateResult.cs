using HR.Data.DTO.Enums;
using HR.Data.Enums;
using HR.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.EmployeeLeaves.Results
{
    public class EmployeeLeavesUpdateResult
    {
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string AttachmentFile { get; set; }
        public string RejectReason { get; set; }
        public LeaveStatus Status { get; set; }
        public EmployeeLeavesValidationStatus ValidationStatus { get; set; }
    }
}
