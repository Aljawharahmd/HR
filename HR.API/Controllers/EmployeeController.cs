using HR.Application.Services.Abstraction;
using HR.Data.DTO.Employees;
using HR.Data.DTO.Employees.Parameters;
using HR.Data.DTO.Employees.Results;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HR.API.Controllers
{
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _employeeService.Get(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }
        [HttpGet("search")]
        public async Task<IActionResult> Get([FromQuery] EmployeeSearchParametrs parametrs)
        {
            var employees = await _employeeService.Get(parametrs);
            if (employees == null) return NotFound();
            return Ok(employees);
        }
        [HttpPost("create")]
        public async Task<EmployeeCreateResult> Post(EmployeeCreateParameters parameters)
        {
            return await _employeeService.Register(parameters);
        }

        [HttpPut("update")]
        public async Task<EmployeeUpdateResult> Put(int id, EmployeeCreateParameters parameters)//change parameters type
        {
            return await _employeeService.Update(id, parameters);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeService.Delete(id);
            if (result != 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
