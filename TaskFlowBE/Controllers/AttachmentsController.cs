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

        //[HttpPost("upload")]
        //public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string? taskId, [FromForm] string? commentId, [FromForm] string uploadedBy)
        //{
        //    var result = await _service.UploadAsync(file, taskId, commentId, uploadedBy, _env.ContentRootPath);
        //    return Ok(result);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id, _env.ContentRootPath);
            return NoContent();
        }
    }
}
