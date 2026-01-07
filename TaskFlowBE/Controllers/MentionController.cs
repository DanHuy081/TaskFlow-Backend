using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MentionController : ControllerBase
    {
        private readonly IMentionService _service;

        public MentionController(IMentionService service)
        {
            _service = service;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return Ok(new List<object>());
            var data = await _service.SearchAsync(query);
            return Ok(data);
        }
    }
}
