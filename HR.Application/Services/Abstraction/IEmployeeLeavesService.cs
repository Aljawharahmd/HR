using HR.Data.DTO.EmployeeLeaves.Parameters;
using HR.Data.DTO.EmployeeLeaves.Results;
using HR.Data.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Services.Abstraction
{
    public interface IEmployeeLeavesService
    {
        Task<List<EmployeeLeavesViewResult>> Get();
        Task<List<EmployeeLeavesViewResult>> Get(EmployeeLeavesSearchParameters parametrs);
        Task<EmployeeLeavesCreateResult> Create(EmployeeLeavesCreateParameters parameters);
        Task<EmployeeLeavesUpdateResult> Update(int id, EmployeeLeavesUpdateParameters parameters);
        Task<int> Delete(int id);
    }
}
