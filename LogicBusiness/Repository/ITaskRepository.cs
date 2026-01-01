using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskFL>> GetAllAsync();
        Task<TaskFL> GetByIdAsync(string id);
        Task AddAsync(TaskFL task);
        Task UpdateAsync(TaskFL task);
        Task DeleteAsync(string id);
        Task<IEnumerable<TaskFL>> GetByListAsync(string listId);
        Task<List<TaskFL>> GetTasksByUserIdAsync(string userId);
        // Tìm các task có hạn chót nằm trong khoảng thời gian (start, end) và chưa hoàn thành
        Task<IEnumerable<TaskFL>> GetTasksDueInIntervalAsync(DateTime start, DateTime end);

        Task<IEnumerable<TaskFL>> GetTasksByTeamIdAsync(string teamId, int take = 20);
        Task<IEnumerable<TaskFL>> GetTasksByListIdAsync(string listId, int take = 20);
    }
}
