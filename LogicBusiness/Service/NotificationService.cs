using CoreEntities.Model;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;

        public NotificationService(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Notification>> GetUserNotifications(Guid userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task CreateNotification(Guid userId, string title, string message)
        {
            var noti = new Notification
            {
                // Nếu DB không tự gán DEFAULT NEWID(), bạn có thể mở dòng dưới:
                // Id = Guid.NewGuid(), 
                UserId = userId,
                Title = title,
                Message = message,
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            await _repository.AddAsync(noti);
        }

        public async Task ReadNotification(Guid id)
        {
            await _repository.MarkAsReadAsync(id);
        }
    }
}
