using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotifications(Guid userId);
        Task CreateNotification(Guid userId, string title, string message);
        Task ReadNotification(Guid id);
    }
}
