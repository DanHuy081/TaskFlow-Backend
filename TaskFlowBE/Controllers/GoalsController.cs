using CoreEntities.Model;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalService _service;

        public GoalsController(IGoalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetByTeam(string teamId)
        {
            return Ok(await _service.GetByTeamIdAsync(teamId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var goal = await _service.GetByIdAsync(id);
            if (goal == null) return NotFound();
            return Ok(goal);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GoalFL goal)
        {
            await _service.AddAsync(goal);
            return Ok(goal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] GoalFL goal)
        {
            if (id != goal.GoalId)
                return BadRequest("Mismatched GoalId");

            await _service.UpdateAsync(goal);
            return Ok(goal);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
