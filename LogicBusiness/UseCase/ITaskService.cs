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
        Task<TaskDto> CreateAsync(TaskCreateDto dto);
        Task<TaskDto> UpdateAsync(string id, TaskUpdateDto dto);
        Task DeleteTaskAsync(string id);
        Task<TaskDto?> UpdateStatusAsync(string id, TaskStatusUpdateDto dto);

    }
}
