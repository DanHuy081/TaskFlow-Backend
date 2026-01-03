using CoreEntities.Model.DTOs;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/public-chat")]
    [ApiController]
    public class PublicChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public PublicChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] PublicChatRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message is required");

            try
            {
                var reply = await _chatService.ProcessPublicChatAsync(request.Message, request.SessionId);
                return Ok(new { reply });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
