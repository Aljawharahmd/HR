using HR.Data.DTO.Enums;
using HR.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.EmployeeLeaves.Results
{
    public class EmployeeLeavesCreateResult
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AttachmentFile { get; set; }
        public EmployeeLeavesValidationStatus ValidationStatus { get; set; }
        public LeaveStatus LeaveStatus { get; set; }

    }
}
