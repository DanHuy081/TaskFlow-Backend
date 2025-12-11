using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("{checklistId}")]
        public async Task<IActionResult> GetByChecklist(string checklistId)
        {
            var result = await _service.GetByChecklistIdAsync(checklistId);
            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(string id)
        //{
        //    var item = await _service.GetByIdAsync(id);
        //    if (item == null) return NotFound();
        //    return Ok(item);
        //}

        [HttpPost]
        public async Task<IActionResult> Create(CreateChecklistItemDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
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

        [HttpPut("{itemId}/toggle")]
        public async Task<IActionResult> Toggle(string itemId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.ToggleResolvedAsync(itemId, userId);
            return NoContent();
        }
    }
}
