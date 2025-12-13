using AutoMapper;
using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class TaskAssigneeService : ITaskAssigneeService
    {
        private readonly ITaskAssigneeRepository _repo;
        private readonly IMapper _mapper;

        public TaskAssigneeService(ITaskAssigneeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskAssignee>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<List<TaskAssigneeDto>> GetByTaskIdAsync(string taskId)
        {
            var data = await _repo.GetByTaskIdAsync(taskId);
            return _mapper.Map<List<TaskAssigneeDto>>(data);
        }

        public async Task<IEnumerable<TaskAssignee>> GetByUserIdAsync(string userId)
            => await _repo.GetByUserIdAsync(userId);

        public async Task<TaskAssignee> GetAsync(string taskId, string userId)
            => await _repo.GetAsync(taskId, userId);

        public async Task<TaskAssigneeDto> AssignUserAsync(TaskAssigneeCreateDto dto)
        {
            var newEntity = new TaskAssignee
            {
                TaskId = dto.TaskId,
                UserId = dto.UserId,
                AssignedAt = DateTime.UtcNow
            };

            var result = await _repo.AddAsync(newEntity);
            return _mapper.Map<TaskAssigneeDto>(result);
        }

        public async Task<bool> UnassignUserAsync(string taskId, string userId)
        {
            return await _repo.DeleteAsync(taskId, userId);
        }
    }

}
