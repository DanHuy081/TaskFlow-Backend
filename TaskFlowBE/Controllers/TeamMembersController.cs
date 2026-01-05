using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.Service;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMembersController : ControllerBase
    {
        private readonly ITeamMemberService _service;

        public TeamMembersController(ITeamMemberService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var members = await _service.GetAllAsync();
            return Ok(members);
        }

        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetByTeam(string teamId)
        {
            var members = await _service.GetByTeamIdAsync(teamId);
            return Ok(members);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var members = await _service.GetByUserIdAsync(userId);
            return Ok(members);
        }

        [HttpGet("{teamId}/{userId}")]
        public async Task<IActionResult> Get(string teamId, string userId)
        {
            var member = await _service.GetAsync(teamId, userId);
            if (member == null) return NotFound();
            return Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TeamMember member)
        {
            await _service.AddAsync(member);
            return Ok(member);
        }

        [HttpPut("{teamId}/{userId}")]
        public async Task<IActionResult> Update(string teamId, string userId, [FromBody] TeamMember member)
        {
            if (member.TeamId != teamId || member.UserId != userId)
                return BadRequest("Mismatched IDs");
            await _service.UpdateAsync(member);
            return Ok(member);
        }

        [HttpDelete("{teamId}/{userId}")]
        public async Task<IActionResult> Delete(string teamId, string userId)
        {
            await _service.DeleteAsync(teamId, userId);
            return NoContent();
        }

        [HttpGet("{teamId}/{userId}/permissions")]
        public async Task<IActionResult> GetPermissions(string teamId, string userId)
        {
            var result = await _service.GetPermissionsAsync(teamId, userId);
            if (result == null) return NotFound("Member not found");

            return Ok(result); // Lúc này JSON trả về sẽ full field
        }

        // PUT: api/TeamMember/update-permissions
        [HttpPut("{teamId}/{userId}/permissions")]
        public async Task<IActionResult> UpdatePermissions([FromBody] TeamMemberUpdateRoleDto request)
        {
            try
            {
                var result = await _service.UpdatePermissionsAsync(request);
                // Trả về luôn object kết quả thay vì chỉ message string
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
