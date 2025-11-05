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
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
       

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            
        }
        public async Task<IEnumerable<TaskFL>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task<TaskFL> GetTaskByIdAsync(string id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task CreateTaskAsync(TaskFL task)
        {
            task.Id = Guid.NewGuid().ToString();
            task.DateCreated = System.DateTime.UtcNow;
            await _taskRepository.AddAsync(task);
        }

        public async Task UpdateTaskAsync(TaskFL task)
        {
            task.DateUpdated = System.DateTime.UtcNow;
            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(string id)
        {
            await _taskRepository.DeleteAsync(id);
        }
    }
}
