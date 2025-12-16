using CoreEntities.Model.DTOs;
using CoreEntities.Model;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskFlowBE.Controllers
{
    [ApiController]
    [Route("api/calendar")]
    [Authorize]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _service;

        public CalendarController(ICalendarService service)
        {
            _service = service;
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyCalendar(
            DateTime from,
            DateTime to)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.GetMyCalendarAsync(userId, from, to);
            return Ok(result);
        }

        [HttpPut("task/{taskId}/move")]
        public async Task<IActionResult> MoveTask(
            string taskId,
            MoveTaskCalendarDto dto)
        {
            await _service.MoveTaskAsync(taskId, dto);
            return Ok();
        }
    }

}
