using HR.Data.Enums;
using System.Collections.Generic;

namespace HR.Data.Models
{
    public class LeaveCategory
    {
        public int Id { get; set; }
        public LeaveType Name { get; set; }
        public int MaxDuration { get; set; }
        public int Balance { get; set; }

        public List<EmployeeLeave> Leaves { get; set; }
    }
}
