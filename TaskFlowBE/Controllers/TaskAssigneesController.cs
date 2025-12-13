using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskAssigneesController : ControllerBase
    {
        private readonly ITaskAssigneeService _service;

        public TaskAssigneesController(ITaskAssigneeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetByTask(string taskId)
        {
            var result = await _service.GetByTaskIdAsync(taskId);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            return Ok(await _service.GetByUserIdAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> AssignUser(TaskAssigneeCreateDto dto)
        {
            var result = await _service.AssignUserAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{taskId}/{userId}")]
        public async Task<IActionResult> Unassign(string taskId, string userId)
        {
            var success = await _service.UnassignUserAsync(taskId, userId);
            if (!success) return NotFound();
            return Ok(new { message = "User unassigned successfully." });
        }
    }
}
