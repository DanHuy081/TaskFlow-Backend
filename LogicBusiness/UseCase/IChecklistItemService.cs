using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface IChecklistItemService
    {
        Task<IEnumerable<ChecklistItemFL>> GetAllAsync();
        Task<IEnumerable<ChecklistItemFL>> GetByChecklistIdAsync(string checklistId);
        Task<ChecklistItemFL> GetByIdAsync(string id);
        Task AddAsync(ChecklistItemFL item);
        Task UpdateAsync(ChecklistItemFL item);
        Task DeleteAsync(string id);
    }
}
