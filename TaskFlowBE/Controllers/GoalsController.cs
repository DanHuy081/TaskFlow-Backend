using CoreEntities.Model;
using CoreEntities.Model.DTOs;
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
        public async Task<ActionResult> Create(GoalCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{goalId}")]
        public async Task<ActionResult> Update(string goalId, GoalUpdateDto dto)
        {
            var ok = await _service.UpdateAsync(goalId, dto);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("{goalId}")]
        public async Task<ActionResult> Delete(string goalId)
        {
            var ok = await _service.DeleteAsync(goalId);
            return ok ? Ok() : NotFound();
        }
    }
}
