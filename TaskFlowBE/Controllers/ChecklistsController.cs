using CoreEntities.Model;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistsController : ControllerBase
    {
        private readonly IChecklistService _service;

        public ChecklistsController(IChecklistService service)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var checklist = await _service.GetByIdAsync(id);
            if (checklist == null) return NotFound();
            return Ok(checklist);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChecklistFL checklist)
        {
            await _service.AddAsync(checklist);
            return Ok(checklist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ChecklistFL checklist)
        {
            if (id != checklist.ChecklistId)
                return BadRequest("ChecklistId mismatch");

            await _service.UpdateAsync(checklist);
            return Ok(checklist);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
