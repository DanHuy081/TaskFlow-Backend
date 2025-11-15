using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface ITaskCustomFieldValueService
    {
        Task<IEnumerable<TaskCustomFieldValueFL>> GetByTaskAsync(string taskId);
        Task<TaskCustomFieldValueFL?> GetAsync(string taskId, string fieldId);
        Task<TaskCustomFieldValueFL> CreateAsync(TaskCustomFieldValueFL data);
        Task<TaskCustomFieldValueFL> UpdateAsync(TaskCustomFieldValueFL data);
        Task<bool> DeleteAsync(string taskId, string fieldId);
    }
}
