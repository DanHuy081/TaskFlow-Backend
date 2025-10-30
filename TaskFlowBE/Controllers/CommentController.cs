using Microsoft.AspNetCore.Mvc;
using CoreEntities.Model;
using LogicBusiness.Service;
using LogicBusiness.UseCase;

namespace TaskFlowBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetCommentsByTaskId(int taskId)
        {
            var comments = await _service.GetCommentsByTaskIdAsync(taskId);
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _service.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {
            var created = await _service.AddCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] Comment comment)
        {
            if (id != comment.Id)
                return BadRequest("CommentId mismatch");

            var updated = await _service.UpdateCommentAsync(comment);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var deleted = await _service.DeleteCommentAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
