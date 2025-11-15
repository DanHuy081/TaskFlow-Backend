using CoreEntities.Model;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskCustomFieldValuesController : ControllerBase
    {
        private readonly ITaskCustomFieldValueService _service;

        public TaskCustomFieldValuesController(ITaskCustomFieldValueService service)
        {
            _service = service;
        }

        // Get all custom field values for 1 task
        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTask(string taskId)
            => Ok(await _service.GetByTaskAsync(taskId));

        // Get a specific field of a task
        [HttpGet("{taskId}/{fieldId}")]
        public async Task<IActionResult> Get(string taskId, string fieldId)
        {
            var data = await _service.GetAsync(taskId, fieldId);
            return data == null ? NotFound() : Ok(data);
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCustomFieldValueFL data)
            => Ok(await _service.CreateAsync(data));

        // Update
        [HttpPut("{taskId}/{fieldId}")]
        public async Task<IActionResult> Update(string taskId, string fieldId, [FromBody] TaskCustomFieldValueFL data)
        {
            if (taskId != data.TaskId || fieldId != data.FieldId)
                return BadRequest("Composite Key mismatch");

            return Ok(await _service.UpdateAsync(data));
        }

        // Delete
        [HttpDelete("{taskId}/{fieldId}")]
        public async Task<IActionResult> Delete(string taskId, string fieldId)
            => Ok(await _service.DeleteAsync(taskId, fieldId));
    }
}
