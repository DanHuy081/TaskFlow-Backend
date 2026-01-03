using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<TaskFL> GetTaskByIdAsync(string id);
        Task<TaskDto> CreateAsync(TaskCreateDto dto, string userId);
        Task<TaskDto?> UpdateAsync(string id, TaskUpdateDto dto, string userId);
        Task DeleteTaskAsync(string id, string userId);
        Task<TaskDto?> UpdateStatusAsync(string id, TaskStatusUpdateDto dto, string userId);
        Task<IEnumerable<TaskDto>> GetByListAsync(string listId);
        Task<List<TaskFL>> GetTasksByUserIdAsync(string userId);
        Task<IEnumerable<TaskFL>> GetTasksByTeamIdAsync(string teamId, int take = 20);
        Task<IEnumerable<TaskFL>> GetTasksByListIdAsync(string listId, int take = 20);
    }
}
