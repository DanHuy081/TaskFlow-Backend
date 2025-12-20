using CoreEntities.Model.DTOs;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _service;
        private readonly IWebHostEnvironment _env;

        public AttachmentsController(IAttachmentService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTask(string taskId)
        {
            var list = await _service.GetByTaskIdAsync(taskId);
            return Ok(list);
        }

        [HttpGet("comment/{commentId}")]
        public async Task<IActionResult> GetByComment(string commentId)
        {
            var list = await _service.GetByCommentIdAsync(commentId);
            return Ok(list);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")] // Quan trọng để Swagger hiển thị nút upload
        public async Task<IActionResult> Upload([FromForm] UploadFileDto request)
        {
            try
            {
                // Gọi service để lưu file vật lý
                string filePath = await _service.UploadAsync(request.File, "attachments");

                // TODO: Tại đây bạn có thể gọi thêm Repository để lưu filePath, TaskId, CommentId vào Database
                // Ví dụ: await _attachmentRepo.AddAsync(new Attachment { Url = filePath, TaskId = request.TaskId ... });

                return Ok(new
                {
                    message = "Upload thành công",
                    url = filePath,
                    taskId = request.TaskId // Trả lại để kiểm tra chơi
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id, _env.ContentRootPath);
            return NoContent();
        }
    }
}
