using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.Service;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Team = CoreEntities.Model.Team;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Member, Guest")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _service;

        public TeamsController(ITeamService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _service.GetAllAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var team = await _service.GetByIdAsync(id);
            if (team == null) return NotFound();
            return Ok(team);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto dto)
        {
            // 1. Validate userId
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // 2. Validate input
            if (string.IsNullOrEmpty(dto.Name)) return BadRequest("Tên Team là bắt buộc");

            try
            {
                // 3. Gọi Service
                var createdTeam = await _service.CreateTeamAsync(dto, userId);

                // 4. Trả về kết quả
                return Ok(createdTeam);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Team team)
        {
            if (id != team.TeamId) return BadRequest("Mismatched ID");
            await _service.UpdateAsync(team);
            return Ok(team);
        }

        [HttpDelete("{teamId}")]
        public async Task<IActionResult> DeleteTeam(string teamId)
        {
            await _service.DeleteTeamCascadeAsync(teamId);
            return NoContent();
        }

        [HttpPost("{teamId}/members")]
        public async Task<IActionResult> AddMember(string teamId, [FromBody] AddMemberDto dto)
        {
            try
            {
                // Gọi Service để xử lý
                await _service.AddUserToTeamAsync(teamId, dto.Email, "Member");

                return Ok(new { message = "Thêm thành viên thành công!" });
            }
            catch (Exception ex)
            {
                // Nếu Service báo lỗi (VD: Email ko tồn tại), trả về 400
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
