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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Comment comment)
        {
            await _service.AddCommentAsync(comment);
            return CreatedAtAction(nameof(GetById), new { id = comment.CommentId }, comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Comment comment)
        {
            if (id != comment.CommentId)
                return BadRequest("ID mismatch.");

            await _service.UpdateCommentAsync(comment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}
