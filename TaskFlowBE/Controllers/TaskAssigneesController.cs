using CoreEntities.Model;
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

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTask(string taskId)
        {
            return Ok(await _service.GetByTaskIdAsync(taskId));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            return Ok(await _service.GetByUserIdAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TaskAssignee assignee)
        {
            await _service.AddAsync(assignee);
            return Ok(assignee);
        }

        [HttpDelete("{taskId}/{userId}")]
        public async Task<IActionResult> Delete(string taskId, string userId)
        {
            await _service.DeleteAsync(taskId, userId);
            return NoContent();
        }
    }
}
