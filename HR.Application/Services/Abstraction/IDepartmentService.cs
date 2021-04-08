using HR.Data.DTO.Departments.Parameters;
using HR.Data.DTO.Departments.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR.Application.Services.Abstraction
{
    public interface IDepartmentService
    {
    //    Task<DepartmentViewResult> Get(int id);
        Task<DepartmentViewResult> Get(DepartmentSearchParameters parameters);
        Task<DepartmentCreateResult> Create(DepartmentCreateParameters parameters);
        Task<DepartmentUpdateResult> Update(int id, DepartmentCreateParameters parameters);
        Task<int> Delete(int id);
    }
}
