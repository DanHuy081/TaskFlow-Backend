using Microsoft.AspNetCore.Mvc;
using CoreEntities.Model;
using LogicBusiness.Service;
using LogicBusiness.UseCase;
using CoreEntities.Model.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TaskFlowBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Member, Guest")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _service.GetAllCommentsAsync();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var comment = await _service.GetCommentByIdAsync(id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTaskId(string taskId)
        {
            var comments = await _service.GetCommentsByTaskIdAsync(taskId);
            return Ok(comments);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(CommentCreateDto dto)
        //{
        //    var result = await _service.CreateAsync(dto);
        //    return Ok(result);
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, CommentUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteCommentAsync(id);
            return NoContent();
        }

        [HttpPost]
        [Authorize] // Bắt buộc đăng nhập
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto dto)
        {
            // Lấy ID người đang đăng nhập từ Token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _service.AddCommentAsync(dto, userId);
            return Ok(new { message = "Comment thành công" });
        }
    }
}
