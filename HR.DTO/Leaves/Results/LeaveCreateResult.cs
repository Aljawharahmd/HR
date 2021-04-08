using HR.Data.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.Leaves.Results
{
    public class LeaveCreateResult
    {
        public LeaveCreateResult()
        {
        }

        public LeaveCreateResult(LeaveValidationStatus status)
        {
            Status = status;
        }
        public LeaveViewResult Data { get; set; }

        public LeaveValidationStatus Status { get; set; }
    }
}
