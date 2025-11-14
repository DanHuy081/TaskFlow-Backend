using CoreEntities.Model;
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

        public TaskAssigneeService(ITaskAssigneeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TaskAssignee>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<TaskAssignee>> GetByTaskIdAsync(string taskId)
            => await _repo.GetByTaskIdAsync(taskId);

        public async Task<IEnumerable<TaskAssignee>> GetByUserIdAsync(string userId)
            => await _repo.GetByUserIdAsync(userId);

        public async Task<TaskAssignee> GetAsync(string taskId, string userId)
            => await _repo.GetAsync(taskId, userId);

        public async Task AddAsync(TaskAssignee assignee)
        {
            assignee.AssignedAt = DateTime.UtcNow;
            await _repo.AddAsync(assignee);
        }

        public async Task DeleteAsync(string taskId, string userId)
            => await _repo.DeleteAsync(taskId, userId);
    }

}
