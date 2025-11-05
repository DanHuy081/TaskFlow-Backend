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
        Task<TaskFL> GetTaskByIdAsync(string id);
        Task CreateTaskAsync(TaskFL task);
        Task UpdateTaskAsync(TaskFL task);
        Task DeleteTaskAsync(string id);
    }
}
