using CoreEntities.Model;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Guest, Member")]
    public class SpacesController : ControllerBase
    {
        private readonly ISpaceService _service;

        public SpacesController(ISpaceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var spaces = await _service.GetAllAsync();
            return Ok(spaces);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var space = await _service.GetByIdAsync(id);
            if (space == null) return NotFound();
            return Ok(space);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Space space)
        {
            await _service.AddAsync(space);
            return Ok(space);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Space space)
        {
            if (id != space.SpaceId) return BadRequest("Mismatched ID");
            await _service.UpdateAsync(space);
            return Ok(space);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
