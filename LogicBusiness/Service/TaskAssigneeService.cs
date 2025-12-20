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
    public class TaskAssigneeService : ITaskAssigneeService
    {
        private readonly ITaskAssigneeRepository _repo;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        // 1. Thêm Repository của Task để lấy thông tin Task
        private readonly ITaskRepository _taskRepository;

        public TaskAssigneeService(
            ITaskAssigneeRepository repo,
            IMapper mapper,
            INotificationService notificationService,
            ITaskRepository taskRepository) // 2. Inject vào Constructor
        {
            _repo = repo;
            _mapper = mapper;
            _notificationService = notificationService;
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskAssignee>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<List<TaskAssigneeDto>> GetByTaskIdAsync(string taskId)
        {
            var assignees = await _repo.GetByTaskIdAsync(taskId);

            return assignees.Select(a => new TaskAssigneeDto
            {
                UserId = a.UserId,
                AssignedAt = (DateTime)a.AssignedAt,
                User = a.UserFLs == null ? null : new UserMiniDto
                {
                    UserId = a.UserFLs.UserId,
                    FullName = a.UserFLs.FullName,
                    Email = a.UserFLs.Email,
                    Color = a.UserFLs.Color,
                    ProfilePicture = a.UserFLs.ProfilePicture
                }
            }).ToList();
        }

        public async Task<IEnumerable<TaskAssignee>> GetByUserIdAsync(string userId)
            => await _repo.GetByUserIdAsync(userId);

        public async Task<TaskAssignee> GetAsync(string taskId, string userId)
            => await _repo.GetAsync(taskId, userId);

        public async Task<TaskAssigneeDto> AssignUserAsync(TaskAssigneeCreateDto dto)
        {
            // 1. Lưu người được giao vào DB
            var newEntity = new TaskAssignee
            {
                TaskId = dto.TaskId,
                UserId = dto.UserId,
                AssignedAt = DateTime.UtcNow
            };

            var result = await _repo.AddAsync(newEntity);

            // 2. Xử lý thông báo (Notification)
            try
            {
                if (Guid.TryParse(dto.UserId, out Guid userGuid))
                {
                    // --- BƯỚC MỚI: Lấy thông tin Task để lấy tên ---
                    // Lưu ý: Đảm bảo _taskRepository có hàm GetAsync hoặc GetByIdAsync nhận vào string/Guid
                    var taskInfo = await _taskRepository.GetByIdAsync(dto.TaskId);

                    string taskName = "công việc mới";
                    if (taskInfo != null)
                    {
                        // Giả sử property tên task là TaskName hoặc Title
                        taskName = taskInfo.Name;
                    }

                    string title = "Bạn được phân công công việc mới";
                    string message = $"Bạn đã được thêm vào thẻ: {taskName}. Hãy kiểm tra ngay!";

                    await _notificationService.CreateNotification(userGuid, title, message);
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần, không throw exception để tránh lỗi luồng chính
                Console.WriteLine($"Lỗi gửi thông báo: {ex.Message}");
            }

            return _mapper.Map<TaskAssigneeDto>(result);
        }

        public async Task<bool> UnassignUserAsync(string taskId, string userId)
        {
            return await _repo.DeleteAsync(taskId, userId);
        }
    }
}