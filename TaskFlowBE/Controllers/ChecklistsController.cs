using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Guest, Member")]
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
            var result = await _service.GetByTaskIdAsync(taskId);
            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(string id)
        //{
        //    var checklist = await _service.GetByIdAsync(id);
        //    if (checklist == null) return NotFound();
        //    return Ok(checklist);
        //}

        [HttpPost]
        public async Task<IActionResult> Create(CreateChecklistDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
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
