using CoreEntities.Model.DTOs;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        // GET: api/notification/user/3fa85f64-5717-4562-b3fc-2c963f66afa6
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var result = await _service.GetUserNotifications(userId);
            return Ok(result);
        }

        // PUT: api/notification/read/3fa85f64-5717-4562-b3fc-2c963f66afa6
        [HttpPut("read/{id}")]
        public async Task<IActionResult> MarkRead(Guid id)
        {
            await _service.ReadNotification(id);
            return Ok(new { message = "Marked as read" });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationDto request)
        {
            await _service.CreateNotification(request.UserId, request.Title, request.Message);
            return Ok(new { message = "Notification created successfully" });
        }
    }

    // Cập nhật DTO
    
}
