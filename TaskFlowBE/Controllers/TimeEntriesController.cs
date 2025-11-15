using CoreEntities.Model;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ITimeEntryService _service;

        public TimeEntriesController(ITimeEntryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TimeEntryFL entry)
            => Ok(await _service.CreateAsync(entry));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TimeEntryFL entry)
        {
            if (id != entry.TimeEntryId) return BadRequest("ID mismatch");
            return Ok(await _service.UpdateAsync(entry));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
            => Ok(await _service.DeleteAsync(id));
    }
}
