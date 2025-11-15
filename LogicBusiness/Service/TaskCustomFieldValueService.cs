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
    public class TaskCustomFieldValueService : ITaskCustomFieldValueService
    {
        private readonly ITaskCustomFieldValueRepository _repo;

        public TaskCustomFieldValueService(ITaskCustomFieldValueRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<TaskCustomFieldValueFL>> GetByTaskAsync(string taskId)
            => _repo.GetByTaskIdAsync(taskId);

        public Task<TaskCustomFieldValueFL?> GetAsync(string taskId, string fieldId)
            => _repo.GetAsync(taskId, fieldId);

        public Task<TaskCustomFieldValueFL> CreateAsync(TaskCustomFieldValueFL data)
        {
            data.DateUpdated = DateTime.UtcNow;
            return _repo.CreateAsync(data);
        }

        public Task<TaskCustomFieldValueFL> UpdateAsync(TaskCustomFieldValueFL data)
        {
            data.DateUpdated = DateTime.UtcNow;
            return _repo.UpdateAsync(data);
        }

        public Task<bool> DeleteAsync(string taskId, string fieldId)
            => _repo.DeleteAsync(taskId, fieldId);
    }
}
