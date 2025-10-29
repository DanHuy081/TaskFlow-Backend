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
        Task<IEnumerable<TaskFL>> GetAllTasksAsync();
        Task<TaskFL> UpdateTaskAsync(TaskFL task);
        Task<TaskFL> GetTaskByIdAsync(int id);
        Task<bool> DeleteTaskAsync(int id);
        Task<TaskDto> CreateTaskAsync(TaskDto taskDto);
    }
}
