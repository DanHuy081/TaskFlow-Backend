using CoreEntities.Model;
using CoreEntities.Model.DTOs;
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
        Task<IEnumerable<ChecklistItemDto>> GetByChecklistIdAsync(string checklistId);
        Task<ChecklistItemFL> GetByIdAsync(string id);
        Task<ChecklistItemDto> CreateAsync(CreateChecklistItemDto dto);
        Task ToggleResolvedAsync(string itemId, string userId);
        Task UpdateAsync(ChecklistItemFL item);
        Task DeleteAsync(string id);
    }
}
