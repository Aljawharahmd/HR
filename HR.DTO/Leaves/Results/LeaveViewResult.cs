using HR.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.Leaves.Results
{
    public class LeaveViewResult
    {
        public int Id { get; set; }
        public LeaveType Name { get; set; }
        public int MaxDuration { get; set; }
        public int Balance { get; set; }
    }
}
