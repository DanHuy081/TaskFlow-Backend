using AutoMapper;
using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.Helpers;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notifRepo;
        private readonly IUserRepository _userRepo; // Để lấy tên người comment

        public CommentService(ICommentRepository repository, IMapper mapper, INotificationRepository notifRepo, IUserRepository userRepo)
        {
            _repository = repository;
            _mapper = mapper;
            _notifRepo = notifRepo;
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByTaskIdAsync(string taskId)
        {
            return await _repository.GetByTaskIdAsync(taskId);
        }

        public async Task<CommentDto> CreateAsync(CommentCreateDto dto)
        {
            var entity = _mapper.Map<Comment>(dto);
            entity.CommentId = Guid.NewGuid().ToString();
            entity.IsEdited = false;
            entity.DateCreated = DateTime.UtcNow;
            entity.DateUpdated = DateTime.UtcNow;

            await _repository.CreateAsync(entity);

            return _mapper.Map<CommentDto>(entity);
        }

        public async Task<CommentDto?> UpdateAsync(string id, CommentUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;


            _mapper.Map(dto, entity);
            entity.IsEdited = true;
            entity.DateUpdated = DateTime.UtcNow;


            await _repository.UpdateAsync(entity);
            return _mapper.Map<CommentDto>(entity);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task AddCommentAsync(CommentCreateDto dto, string currentUserId)
        {
            // 1. Lưu Comment vào DB (Lưu chuỗi RAW có chứa @[...](...))
            var comment = new Comment
            {
                CommentId = Guid.NewGuid().ToString(),
                CommentText = dto.CommentText,
                TaskId = dto.TaskId,
                UserId = currentUserId,
                //CreatedDate = DateTime.UtcNow
            };
            await _repository.CreateAsync(comment);

            // 2. Xử lý Mention (Gửi thông báo)
            await ProcessMentions(dto.CommentText, dto.TaskId, currentUserId);
        }

        private async Task ProcessMentions(string content, string taskId, string senderId)
        {
            // Dùng Helper parse chuỗi
            var mentions = MentionParser.GetMentions(content);

            if (!mentions.Any()) return;

            // Lấy thông tin người gửi để hiện trong thông báo
            var sender = await _userRepo.GetByIdAsync(senderId);
            string senderName = sender?.FullName ?? "Ai đó";

            foreach (var item in mentions)
            {
                // Chỉ gửi thông báo nếu tag User (Tag Task thì chỉ tạo link, ko cần noti)
                if (Guid.TryParse(item.Id, out Guid targetUserId))
                {
                    var noti = new Notification
                    {
                        Title = "Bạn được nhắc đến",
                        UserId = targetUserId, // Gán Guid đã parse
                        Message = $"<b>{senderName}</b> đã nhắc đến bạn trong một bình luận.",
                        // Link = $"/task-detail/{taskId}",
                        IsRead = false,
                        // Cần đảm bảo CreatedAt hoặc các trường bắt buộc khác được gán nếu DB yêu cầu
                        CreatedAt = DateTime.UtcNow
                    };

                    await _notifRepo.CreateAsync(noti);
                }
            }
        }
    }
}

