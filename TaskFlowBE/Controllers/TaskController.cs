using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlowBE.Data;
using CoreEntities.Model;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using LogicBusiness.UseCase;
using CoreEntities.Model.DTOs;
using AutoMapper;

namespace TaskFlowBE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Route sẽ là: /api/tasks
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public TasksController(ITaskService taskService,  IMapper mapper, ApplicationDbContext context)
        {
            _taskService = taskService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet] // Xử lý HTTP GET request
        [ProducesResponseType(typeof(IEnumerable<TaskFL>), 200)] // Thông báo kiểu trả về thành công
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                // Chỉ cần gọi BLL, BLL sẽ lo phần còn lại
                var tasks = await _taskService.GetAllTasksAsync();

                // Trả về HTTP 200 OK cùng với danh sách DTO
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chung
                return StatusCode(500, "Đã xảy ra lỗi máy chủ nội bộ.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto dto)
        {
            var task = _mapper.Map<TaskFL>(dto); // Map từ DTO sang Entity

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<TaskDto>(task); // Map ngược lại nếu muốn trả ra DTO
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskFL task)
        {
            if (id != task.Id) return BadRequest("ID mismatch.");
            var updated = await _taskService.UpdateTaskAsync(task);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result) return NotFound();
            return Ok("Deleted successfully.");
        }
    }
}