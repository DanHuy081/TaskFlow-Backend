using CoreEntities.Model;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _service;

        public TagsController(ITagService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllTagsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var tag = await _service.GetTagByIdAsync(id);
            if (tag == null) return NotFound();
            return Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Tag tag)
        {
            await _service.AddTagAsync(tag);
            return CreatedAtAction(nameof(GetById), new { id = tag.TagId }, tag);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Tag tag)
        {
            if (id != tag.TagId) return BadRequest();
            await _service.UpdateTagAsync(tag);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteTagAsync(id);
            return NoContent();
        }
    }
}
