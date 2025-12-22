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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadFileDto request)
        {
            // Kiểm tra request
            if (request.File == null || request.File.Length == 0)
                return BadRequest("Vui lòng chọn file.");

            try
            {
                // GỌI HÀM MỚI: Truyền đủ 4 tham số bắt buộc
                // rootPathFromController để null cũng được vì Service tự lấy WebRootPath
                var result = await _service.UploadAsync(
                    request.File,
                    request.TaskId,
                    request.CommentId,
                    request.UploadedBy ?? "UnknownUser" // Xử lý nếu null
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                // GỌI HÀM MỚI: Chỉ truyền 1 tham số là ID
                // (Service đã tự lo việc tìm đường dẫn gốc để xóa file vật lý)
                await _service.DeleteAsync(id);

                return Ok(new { message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
