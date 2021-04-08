using HR.Application.Services.Abstraction;
using HR.Data.DTO.Departments.Parameters;
using HR.Data.DTO.Departments.Results;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HR.API.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery] DepartmentSearchParameters parametrs)
        {
            var department = await _departmentService.Get(parametrs);
            if (department == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(department);
            }
        }
        [HttpPost("create")]
        public async Task<DepartmentCreateResult> Post(DepartmentCreateParameters parameters)
        {
            return await _departmentService.Create(parameters);
        }
        [HttpPut("update")]
        public async Task<DepartmentUpdateResult> Put(int id, DepartmentCreateParameters parameters)
        {
            return await _departmentService.Update(id, parameters);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _departmentService.Delete(id);
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
