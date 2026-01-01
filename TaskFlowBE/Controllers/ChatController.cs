using CoreEntities.Model.DTOs;
using CoreEntities.Model;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bắt buộc đăng nhập
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // 1. Gửi tin nhắn và nhận phản hồi AI
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequestDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _chatService.ProcessChatAsync(request, userId);
            return Ok(response);
        }

        // 2. Lấy danh sách lịch sử chat
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var history = await _chatService.GetHistoryAsync(userId);
            return Ok(history);
        }

        [HttpPost("stream")]
        public async Task Stream([FromBody] ChatRequestDto request)
        {
            Response.Headers["Content-Type"] = "text/event-stream; charset=utf-8";
            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["Connection"] = "keep-alive";
            Response.Headers["X-Accel-Buffering"] = "no"; // nếu chạy qua nginx

            var ct = HttpContext.RequestAborted;

            // userId lấy từ auth/claim hoặc tạm test
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 1) xử lý chat (tạo conversation, lưu db, lấy reply)
            var result = await _chatService.ProcessChatAsync(request, userId);

            // 2) meta (JSON)
            var metaJson = JsonSerializer.Serialize(new
            {
                conversationId = result.ConversationId,
                title = result.Title
            });

            await WriteSseEventAsync("meta", metaJson, ct);

            // 3) delta (text thuần để Swagger/FE dễ đọc)
            var text = result.Reply ?? "";
            foreach (var chunk in SplitIntoChunksByWord(text, 60))
            {
                if (ct.IsCancellationRequested) break;
                await WriteSseEventAsync("delta", chunk, ct);
                await Task.Delay(10, ct);
            }

            // 4) done
            if (!ct.IsCancellationRequested)
                await WriteSseEventAsync("done", "[DONE]", ct);
        }

        private async Task WriteSseEventAsync(string eventName, string data, CancellationToken ct)
        {
            data = (data ?? "").Replace("\r", "").Replace("\n", "\\n");

            await Response.WriteAsync($"event: {eventName}\n", ct);
            await Response.WriteAsync($"data: {data}\n\n", ct);
            await Response.Body.FlushAsync(ct);
        }

        private static IEnumerable<string> SplitIntoChunksByWord(string s, int maxLen = 60)
        {
            if (string.IsNullOrWhiteSpace(s)) yield break;

            var words = s.Split(' ');
            var cur = new StringBuilder();

            foreach (var w in words)
            {
                if (cur.Length + w.Length + 1 > maxLen)
                {
                    yield return cur.ToString();
                    cur.Clear();
                }
                if (cur.Length > 0) cur.Append(' ');
                cur.Append(w);
            }

            if (cur.Length > 0) yield return cur.ToString();
        }

        [HttpDelete("conversation/{conversationId:guid}")]
        public async Task<IActionResult> DeleteConversation(Guid conversationId)
        {
            var userId =
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            await _chatService.DeleteConversationAsync(conversationId, userId);
            return Ok(new { ok = true });
        }

        [HttpGet("conversation/{id:guid}")]
        public async Task<IActionResult> GetConversation(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                var conversation = await _chatService.GetConversationAsync(id, userId);

                // Map sang DTO gọn gàng để trả về FE
                return Ok(new
                {
                    id = conversation.ConversationId,
                    title = conversation.Title,
                    messages = conversation.Messages.Select(m => new
                    {
                        id = m.MessageId,
                        role = m.Role,
                        content = m.Content,
                        timestamp = m.DateCreated
                    }).OrderBy(m => m.timestamp)
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Conversation not found");
            }
        }
    }
}
