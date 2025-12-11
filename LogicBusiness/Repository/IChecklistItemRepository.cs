using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IChecklistItemRepository
    {
        Task<IEnumerable<ChecklistItemFL>> GetAllAsync();
        Task<IEnumerable<ChecklistItemFL>> GetByChecklistIdAsync(string checklistId);
        Task<ChecklistItemFL> GetByIdAsync(string id);
        Task<ChecklistItemFL> CreateAsync(ChecklistItemFL item);
        Task UpdateAsync(ChecklistItemFL item);
        Task DeleteAsync(string id);
        Task ToggleResolvedAsync(string itemId, string userId);
    }
}
