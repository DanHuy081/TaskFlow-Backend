using CoreEntities.Model;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistItemsController : ControllerBase
    {
        private readonly IChecklistItemService _service;

        public ChecklistItemsController(IChecklistItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("checklist/{checklistId}")]
        public async Task<IActionResult> GetByChecklist(string checklistId)
            => Ok(await _service.GetByChecklistIdAsync(checklistId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChecklistItemFL item)
        {
            await _service.AddAsync(item);
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ChecklistItemFL item)
        {
            if (id != item.ChecklistItemId)
                return BadRequest("ChecklistItemId mismatch");

            await _service.UpdateAsync(item);
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
