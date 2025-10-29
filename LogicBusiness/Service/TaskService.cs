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
            // 1. Gọi DAL để lấy dữ liệu Entity
            var tasksFromDb = await _taskRepository.GetAllTasksAsync();

  
            var taskDtos = tasksFromDb.Select(task => new TaskFL
            {
                Id = task.Id,
                ProjectId = task.ProjectId,
                Title = task.Title,
                Description = task.Description,
                AssignedTo = task.AssignedTo,
                CreatedBy = task.CreatedBy,
                Status = task.Status,
                Priority = task.Priority,
                StartDate = task.StartDate,
                DueDate = task.DueDate,
                Progress = task.Progress,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            });

            return taskDtos;
        }

        public async Task<TaskFL> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }
        public async Task<TaskDto> CreateTaskAsync(TaskDto taskDto)
        {
            var task = new TaskFL
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                ProjectId = taskDto.ProjectId,
                AssignedTo = taskDto.AssignedTo,
                Status = taskDto.Status,
                Priority = taskDto.Priority,
                Progress = taskDto.Progress,
                StartDate = taskDto.StartDate,
                DueDate = taskDto.DueDate,
                CreatedBy = taskDto.CreatedBy,
                CreatedAt = DateTime.Now
            };

            var created = await _taskRepository.CreateTaskAsync(task);

            return new TaskDto
            {
                Id = created.Id,
                Title = created.Title,
                Description = created.Description,
                ProjectId = created.ProjectId,
                AssignedTo = created.AssignedTo,
                Status = created.Status,
                Priority = created.Priority,
                Progress = created.Progress,
                StartDate = created.StartDate,
                DueDate = created.DueDate,
                CreatedBy = created.CreatedBy
            };
        }

        public async Task<TaskFL> UpdateTaskAsync(TaskFL task)
        {
            task.UpdatedAt = DateTime.Now;
            return await _taskRepository.UpdateAsync(task);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteAsync(id);
        }
    }
}
