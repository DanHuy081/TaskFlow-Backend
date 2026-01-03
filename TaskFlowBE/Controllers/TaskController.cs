using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlServer.Data;
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
using Microsoft.AspNetCore.Authorization;

namespace TaskFlowBE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Guest, Member")]
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _taskService.GetAllTasksAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskFL>> GetById(string id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 👇 Truyền userId vào Service
            var result = await _taskService.CreateAsync(dto, userId);
            return Ok(result); // trả về DTO
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, TaskUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _taskService.UpdateAsync(id, dto, userId); // Truyền thêm userId

            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _taskService.DeleteTaskAsync(id, userId); // Truyền thêm userId
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] TaskStatusUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _taskService.UpdateStatusAsync(id, dto, userId); // Truyền thêm userId

            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("by-list/{listId}")]
        public async Task<IActionResult> GetByList(string listId)
        {
            var tasks = await _taskService.GetByListAsync(listId);
            return Ok(tasks);
        }
    }
}