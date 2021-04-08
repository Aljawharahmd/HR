using HR.Application.Services.Abstraction;
using HR.Data.DTO.EmployeeLeaves.Parameters;
using HR.Data.DTO.EmployeeLeaves.Results;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HR.API.Controllers
{
    [Route("api/EmployeeLeaves")]
    [ApiController]
    public class EmployeeLeavesController : ControllerBase
    {
        private readonly IEmployeeLeavesService _employeeLeaveService;

        public EmployeeLeavesController(IEmployeeLeavesService leaveService)
        {
            _employeeLeaveService = leaveService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var leave = await _employeeLeaveService.Get();
            if (leave == null)
                return NotFound();

            return Ok(leave);
        }
        [HttpGet("search")]
        public async Task<IActionResult> Get([FromQuery] EmployeeLeavesSearchParameters parametrs)
        {
            var leave = await _employeeLeaveService.Get(parametrs);
            if (leave == null) return NotFound();
            return Ok(leave);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post(EmployeeLeavesCreateParameters parameters)
        {
            var leave = await _employeeLeaveService.Create(parameters);
            if (leave == null) 
                return NotFound();

            return Ok(leave);
        }

        [HttpPut("update")]
        public async Task<EmployeeLeavesUpdateResult> Put(int id, EmployeeLeavesUpdateParameters parameters)
        {
            return await _employeeLeaveService.Update(id, parameters);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeLeaveService.Delete(id);
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
