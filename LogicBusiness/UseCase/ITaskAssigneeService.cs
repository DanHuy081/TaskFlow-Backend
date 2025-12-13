using CoreEntities.Model;
using CoreEntities.Model.DTOs;
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
        Task<List<TaskAssigneeDto>> GetByTaskIdAsync(string taskId);
        Task<IEnumerable<TaskAssignee>> GetByUserIdAsync(string userId);
        Task<TaskAssignee> GetAsync(string taskId, string userId);
        Task<TaskAssigneeDto> AssignUserAsync(TaskAssigneeCreateDto dto);
        Task<bool> UnassignUserAsync(string taskId, string userId);
    }
}
