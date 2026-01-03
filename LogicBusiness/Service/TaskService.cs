using AutoMapper;
using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly IActivityService _activityService;
        private readonly IListRepository _listRepository;
        private readonly ISpaceRepository _spaceRepository;

        public TaskService(ITaskRepository taskRepository, IMapper mapper, IActivityService activityService, IListRepository listRepository, ISpaceRepository spaceRepository)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _activityService = activityService;
            _listRepository = listRepository;
            _spaceRepository = spaceRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<TaskFL> GetTaskByIdAsync(string id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        // 👇 ĐÃ SỬA: Thêm tham số userId
        public async Task<TaskDto> CreateAsync(TaskCreateDto dto, string userId)
        {
            var entity = _mapper.Map<TaskFL>(dto);

            entity.Id = Guid.NewGuid().ToString();
            entity.DateCreated = DateTime.UtcNow;
            entity.DateUpdated = DateTime.UtcNow;
            entity.IsArchived = false;
            entity.Status = dto.Status ?? "TO DO"; // Default status nếu null
            entity.CreatorId = userId; // Lưu người tạo vào Task luôn

            await _taskRepository.AddAsync(entity);

            // --- GHI LOG ---
            // Lưu ý: Cần chuyển ListId (string) sang Guid để lưu vào Log. 
            // Nếu bạn có TeamId trong TaskFL thì thay entity.ListId bằng entity.TeamId
            var contextId = await GetTeamIdFromListAsync(entity.ListId);

            await _activityService.LogAsync(
                userId: userId,
                teamId: contextId,
                action: "CREATE_TASK",
                entityName: "Task",
                entityId: entity.Id,
                description: $"Đã tạo công việc mới: {entity.Name}"
            );

            return _mapper.Map<TaskDto>(entity);
        }

        // 👇 ĐÃ SỬA: Thêm tham số userId và sửa logic log
        public async Task<TaskDto?> UpdateAsync(string id, TaskUpdateDto dto, string userId)
        {
            var entity = await _taskRepository.GetByIdAsync(id);
            if (entity == null) return null;

            // 1. Lưu lại giá trị cũ để so sánh
            string oldStatus = entity.Status;
            string oldName = entity.Name;
            var contextId = await GetTeamIdFromListAsync(entity.ListId);

            // 2. Map dữ liệu mới vào entity
            _mapper.Map(dto, entity);
            entity.DateUpdated = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(entity);

            // 3. Ghi Log thông minh
            if (oldStatus != dto.Status) // So sánh status cũ và mới (trong dto)
            {
                await _activityService.LogAsync(
                    userId, contextId,
                    "UPDATE_STATUS", "Task", id,
                    $"Đã đổi trạng thái từ '{oldStatus}' sang '{dto.Status}' cho task '{oldName}'"
                );
            }
            else if (oldName != dto.Name) // So sánh tên
            {
                await _activityService.LogAsync(
                    userId, contextId,
                    "UPDATE_INFO", "Task", id,
                    $"Đã đổi tên công việc từ '{oldName}' thành '{dto.Name}'"
                );
            }
            else
            {
                await _activityService.LogAsync(
                    userId, contextId,
                    "UPDATE_TASK", "Task", id,
                    $"Đã cập nhật thông tin công việc: {oldName}"
                );
            }

            return _mapper.Map<TaskDto>(entity);
        }

        // 👇 ĐÃ SỬA: Thêm userId và lấy thông tin trước khi xóa
        public async Task DeleteTaskAsync(string id, string userId)
        {
            // 1. Lấy task lên trước khi xóa để biết nó tên là gì
            var entity = await _taskRepository.GetByIdAsync(id);
            if (entity == null) return;

            string taskName = entity.Name;
            var contextId = await GetTeamIdFromListAsync(entity.ListId);

            // 2. Xóa
            await _taskRepository.DeleteAsync(id);

            // 3. Ghi log
            await _activityService.LogAsync(
                userId,
                contextId,
                "DELETE_TASK",
                "Task",
                id,
                $"Đã xóa công việc: {taskName}"
            );
        }

        // 👇 ĐÃ SỬA: Sửa lại logic log bị copy nhầm từ Delete
        public async Task<TaskDto?> UpdateStatusAsync(string id, TaskStatusUpdateDto dto, string userId)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;

            string oldStatus = task.Status;

            // ❌ Code cũ của bạn (Sẽ bị lỗi không hiện log):
            // Guid.TryParse(task.ListId, out Guid contextId);

            // ✅ SỬA THÀNH (Gọi hàm Helper):
            var contextId = await GetTeamIdFromListAsync(task.ListId);

            // ... (Phần update và log giữ nguyên) ...
            task.Status = dto.Status;
            task.DateUpdated = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(task);

            // Ghi log
            if (oldStatus != dto.Status)
            {
                await _activityService.LogAsync(
                    userId,
                    contextId, // Giờ biến này đã chứa đúng TeamId
                    "UPDATE_STATUS",
                    "Task",
                    id,
                    $"Đã cập nhật trạng thái: {task.Name} ({oldStatus} -> {dto.Status})"
                );
            }

            return _mapper.Map<TaskDto>(task);
        }

        // Các hàm Get giữ nguyên
        public async Task<IEnumerable<TaskDto>> GetByListAsync(string listId)
        {
            var tasks = await _taskRepository.GetByListAsync(listId);
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<List<TaskFL>> GetTasksByUserIdAsync(string userId)
        {
            return await _taskRepository.GetTasksByUserIdAsync(userId);
        }

        public async Task<IEnumerable<TaskFL>> GetTasksByTeamIdAsync(string teamId, int take = 20)
        {
            return await _taskRepository.GetTasksByTeamIdAsync(teamId, take);
        }

        public async Task<IEnumerable<TaskFL>> GetTasksByListIdAsync(string listId, int take = 20)
        {
            return await _taskRepository.GetTasksByListIdAsync(listId, take);
        }

        private async Task<Guid> GetTeamIdFromListAsync(string listId)
        {
            if (string.IsNullOrEmpty(listId)) return Guid.Empty;

            // BƯỚC 1: Tìm cái List
            var list = await _listRepository.GetByIdAsync(listId);
            if (list == null) return Guid.Empty;

            // BƯỚC 2: Tìm cái Space (Dựa vào SpaceId trong List)
            // Lưu ý: Kiểm tra xem trong entity List của bạn, khóa ngoại tên là 'SpaceId' hay gì nhé
            var space = await _spaceRepository.GetByIdAsync(list.SpaceId);
            if (space == null) return Guid.Empty;

            // BƯỚC 3: Lấy TeamId từ Space
            // Lưu ý: Kiểm tra xem Space lưu TeamId dạng string hay Guid
            if (Guid.TryParse(space.TeamId, out Guid teamId))
            {
                return teamId;
            }

            return Guid.Empty;
        }
    }
}