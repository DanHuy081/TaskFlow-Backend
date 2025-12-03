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
            var result = await _taskService.CreateAsync(dto);
            return Ok(result); // trả về DTO
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, TaskUpdateDto dto)
        {
            var updated = await _taskService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, TaskStatusUpdateDto dto)
        {
            var updated = await _taskService.UpdateStatusAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

    }
}