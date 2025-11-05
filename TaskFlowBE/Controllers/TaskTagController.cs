using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTagsController : ControllerBase
    {
        private readonly ITaskTagService _service;

        public TaskTagsController(ITaskTagService service)
        {
            _service = service;
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTagsByTask(string taskId)
        {
            var result = await _service.GetTagsByTaskAsync(taskId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddTagToTask(string taskId, string tagId)
        {
            await _service.AddTagToTaskAsync(taskId, tagId);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveTagFromTask(string taskId, string tagId)
        {
            await _service.RemoveTagFromTaskAsync(taskId, tagId);
            return NoContent();
        }
    }
}
