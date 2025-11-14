using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface ITaskAssigneeService
    {
        Task<IEnumerable<TaskAssignee>> GetAllAsync();
        Task<IEnumerable<TaskAssignee>> GetByTaskIdAsync(string taskId);
        Task<IEnumerable<TaskAssignee>> GetByUserIdAsync(string userId);
        Task<TaskAssignee> GetAsync(string taskId, string userId);
        Task AddAsync(TaskAssignee assignee);
        Task DeleteAsync(string taskId, string userId);
    }
}
