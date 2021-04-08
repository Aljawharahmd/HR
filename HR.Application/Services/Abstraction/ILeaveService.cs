using HR.Data.DTO.Leaves.Parameters;
using HR.Data.DTO.Leaves.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Services.Abstraction
{
    public interface ILeaveService
    {
        Task<List<LeaveViewResult>> Get();
        Task<List<LeaveViewResult>> Get(LeaveSearchParameters parametrs);
        Task<LeaveCreateResult> Create(LeaveCreateParameters parameters);
        //Change parameters type
        Task<LeaveUpdateResult> Update(int id, LeaveUpdateParameters parameters);
        Task<int> Delete(int id);
    }
}
