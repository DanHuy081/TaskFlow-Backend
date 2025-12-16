using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface ICalendarRepository
    {
        Task<List<TaskFL>> GetTasksForCalendarAsync(
            string userId,
            DateTime from,
            DateTime to
        );

        Task<TaskFL?> GetTaskByIdAsync(string taskId);
        Task UpdateTaskAsync(TaskFL task);
    }

}
