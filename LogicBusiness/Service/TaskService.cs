using AutoMapper;
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
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();

            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                ListId = t.ListId,
                ParentTaskId = t.ParentTaskId,
                Name = t.Name,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                StartDate = t.StartDate,
                TimeEstimate = t.TimeEstimate,
                TimeSpent = t.TimeSpent,
                CreatorId = t.CreatorId,
                DateCreated = t.DateCreated,
                DateUpdated = t.DateUpdated,
                DateClosed = t.DateClosed,
                IsArchived = t.IsArchived,
                Url = t.Url
            });
        }


        public async Task<TaskFL> GetTaskByIdAsync(string id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<TaskDto> CreateAsync(TaskCreateDto dto)
        {
            // Mapping từ DTO sang Entity (AutoMapper làm hết)
            var entity = _mapper.Map<TaskFL>(dto);

            entity.Id = Guid.NewGuid().ToString();
            entity.DateCreated = DateTime.UtcNow;
            entity.DateUpdated = DateTime.UtcNow;
            entity.IsArchived = false;

            await _taskRepository.AddAsync(entity);

            // Mapping Entity → DTO trả ra
            return _mapper.Map<TaskDto>(entity);
        }



        public async Task<TaskDto?> UpdateAsync(string id, TaskUpdateDto dto)
        {
            var entity = await _taskRepository.GetByIdAsync(id);
            if (entity == null) return null;

            // Map từ UpdateDto sang Entity (ghi đè các field)
            _mapper.Map(dto, entity);

            entity.DateUpdated = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(entity);
            return _mapper.Map<TaskDto>(entity);
        }


        public async Task DeleteTaskAsync(string id)
        {
            await _taskRepository.DeleteAsync(id);
        }
    }
}
