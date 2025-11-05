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
    }
}
