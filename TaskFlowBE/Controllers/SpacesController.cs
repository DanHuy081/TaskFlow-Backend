using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.Service;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize]
        public async Task<IActionResult> CreateSpace(SpaceCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _service.CreateAsync(dto, userId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Space space)
        {
            if (id != space.SpaceId) return BadRequest("Mismatched ID");
            await _service.UpdateAsync(space);
            return Ok(space);
        }

        [HttpDelete("{spaceId}")]
        public async Task<IActionResult> DeleteSpace(string spaceId)
        {
            await _service.DeleteSpaceCascadeAsync(spaceId);
            return NoContent();
        }


        [HttpGet("my-spaces")]
        public async Task<IActionResult> GetMySpaces()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var spaces = await _service.GetMySpacesAsync(userId);

            return Ok(spaces);
        }

        // GET: api/Spaces/personal
        [HttpGet("personal")]
        public async Task<IActionResult> GetPersonalWorkspace()
        {
            // Lấy UserId từ Token (Nó là string)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            try
            {
                var result = await _service.GetPersonalWorkspaceAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
