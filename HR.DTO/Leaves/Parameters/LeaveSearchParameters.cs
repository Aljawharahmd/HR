using HR.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.Leaves.Parameters
{
    public class LeaveSearchParameters
    {
        public int? Id { get; set; }
        public LeaveType Name { get; set; }
    }
}
