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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskFL>>> GetAll()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskFL>> GetById(string id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TaskFL task)
        {
            await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, TaskFL task)
        {
            if (id != task.Id) return BadRequest();
            await _taskService.UpdateTaskAsync(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}