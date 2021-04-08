using HR.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.Leaves.Parameters
{
    public class LeaveCreateParameters
    {
        public LeaveType Name { get; set; }
        public int MaxDuration { get; set; }
        public int Balance { get; set; }
    }
}
