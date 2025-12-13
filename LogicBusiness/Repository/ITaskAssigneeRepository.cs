using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface ITaskAssigneeRepository
    {
        Task<IEnumerable<TaskAssignee>> GetAllAsync();
        Task<IEnumerable<TaskAssignee>> GetByUserIdAsync(string userId);
        Task<TaskAssignee> GetAsync(string taskId, string userId);
        Task<List<TaskAssignee>> GetByTaskIdAsync(string taskId);
        Task<TaskAssignee> AddAsync(TaskAssignee entity);
        Task<bool> DeleteAsync(string taskId, string userId);
    }
}
