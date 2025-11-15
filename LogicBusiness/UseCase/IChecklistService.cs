using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface IChecklistService
    {
        Task<IEnumerable<ChecklistFL>> GetAllAsync();
        Task<IEnumerable<ChecklistFL>> GetByTaskIdAsync(string taskId);
        Task<ChecklistFL> GetByIdAsync(string id);
        Task AddAsync(ChecklistFL checklist);
        Task UpdateAsync(ChecklistFL checklist);
        Task DeleteAsync(string id);
    }
}
