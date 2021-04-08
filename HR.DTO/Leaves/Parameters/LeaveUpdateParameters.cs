using HR.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.Leaves.Parameters
{
    public class LeaveUpdateParameters
    {
        public int MaxDuration { get; set; }
        public int Balance { get; set; }
    }
}
