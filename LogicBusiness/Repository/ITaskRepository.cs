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
        Task<IEnumerable<TaskFL>> GetAllTasksAsync();
        Task<TaskFL> GetByIdAsync(int id);
        Task<TaskFL> CreateTaskAsync(TaskFL task);
        Task<TaskFL> UpdateAsync(TaskFL task);
        Task<bool> DeleteAsync(int id);
    }
}
