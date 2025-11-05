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
    public class TaskTagService : ITaskTagService
    {
        private readonly ITaskTagRepository _repo;

        public TaskTagService(ITaskTagRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TaskTag>> GetTagsByTaskAsync(string taskId)
        {
            return await _repo.GetTagsByTaskAsync(taskId);
        }

        public async Task AddTagToTaskAsync(string taskId, string tagId)
        {
            await _repo.AddTagToTaskAsync(new TaskTag
            {
                TaskId = taskId,
                TagId = tagId
            });
        }

        public async Task RemoveTagFromTaskAsync(string taskId, string tagId)
        {
            await _repo.RemoveTagFromTaskAsync(taskId, tagId);
        }
    }
}
