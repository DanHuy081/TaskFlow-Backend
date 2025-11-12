using CoreEntities.Model;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly IFolderService _service;

        public FoldersController(IFolderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var folders = await _service.GetAllAsync();
            return Ok(folders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var folder = await _service.GetByIdAsync(id);
            if (folder == null) return NotFound();
            return Ok(folder);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Folder folder)
        {
            await _service.AddAsync(folder);
            return Ok(folder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Folder folder)
        {
            if (id != folder.FolderId) return BadRequest("Mismatched ID");
            await _service.UpdateAsync(folder);
            return Ok(folder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
