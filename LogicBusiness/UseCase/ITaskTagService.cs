using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface ITaskTagService
    {
        Task<IEnumerable<TaskTag>> GetTagsByTaskAsync(string taskId);
        Task AddTagToTaskAsync(string taskId, string tagId);
        Task RemoveTagFromTaskAsync(string taskId, string tagId);
    }
}
