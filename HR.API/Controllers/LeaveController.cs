using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR.Application.Services.Abstraction;
using HR.Data.DTO.Leaves.Parameters;
using HR.Data.DTO.Leaves.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR.API.Controllers
{
    [Route("api/leave")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var leave = await _leaveService.Get();
            if (leave == null)
                return NotFound();

            return Ok(leave);
        }
        [HttpGet("search")]
        public async Task<IActionResult> Get([FromQuery] LeaveSearchParameters parametrs)
        {
            var leave = await _leaveService.Get(parametrs);
            if (leave == null) return NotFound();
            return Ok(leave);
        }
        [HttpPost("create")]
        public async Task<LeaveCreateResult> Post(LeaveCreateParameters parameters)
        {
            return await _leaveService.Create(parameters);
        }

        [HttpPut("update")]
        public async Task<LeaveUpdateResult> Put(int id, LeaveUpdateParameters parameters)
        {
            return await _leaveService.Update(id, parameters);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _leaveService.Delete(id);
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
