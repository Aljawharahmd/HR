using HR.Data.DTO.Employees;
using HR.Data.DTO.Employees.Parameters;
using HR.Data.DTO.Employees.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR.Application.Services.Abstraction
{
    public interface IEmployeeService
    {
        Task<EmployeeViewResult> Get(int id);
        Task<List<EmployeeViewResult>> Get(EmployeeSearchParametrs parametrs);
        Task<EmployeeCreateResult> Register(EmployeeCreateParameters parameters);
        //Change parameters type
        Task<EmployeeUpdateResult> Update(int id, EmployeeCreateParameters parameters);
        Task<int> Delete(int id);
    }
}
