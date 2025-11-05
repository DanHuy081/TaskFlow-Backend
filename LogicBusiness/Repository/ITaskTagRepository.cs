using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface ITaskTagRepository
    {
        Task<IEnumerable<TaskTag>> GetTagsByTaskAsync(string taskId);
        Task AddTagToTaskAsync(TaskTag taskTag);
        Task RemoveTagFromTaskAsync(string taskId, string tagId);
    }
}
