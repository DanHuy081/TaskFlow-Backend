using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
        Task<Notification> AddAsync(Notification notification);
        Task<bool> MarkAsReadAsync(Guid notificationId);
        Task CreateAsync(Notification notification);
    }
}
