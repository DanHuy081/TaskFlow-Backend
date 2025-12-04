using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly IListService _service;

        public ListsController(IListService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lists = await _service.GetAllAsync();
            return Ok(lists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var list = await _service.GetByIdAsync(id);
            if (list == null) return NotFound();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ListCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] List list)
        {
            if (id != list.ListId) return BadRequest("Mismatched ID");
            await _service.UpdateAsync(list);
            return Ok(list);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("space/{spaceId}")]
        public async Task<IActionResult> GetBySpace(string spaceId)
        {
            var result = await _service.GetBySpaceAsync(spaceId);
            return Ok(result);
        }

        [HttpGet("folder/{folderId}")]
        public async Task<IActionResult> GetByFolder(string folderId)
        {
            var result = await _service.GetByFolderAsync(folderId);
            return Ok(result);
        }
    }
}
