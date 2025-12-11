using CoreEntities.Model;
using CoreEntities.Model.DTOs;
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
        Task<IEnumerable<ChecklistDto>> GetByTaskIdAsync(string taskId);
        Task<ChecklistFL> GetByIdAsync(string id);
        Task<ChecklistDto> CreateAsync(CreateChecklistDto dto);
        Task UpdateAsync(ChecklistFL checklist);
        Task DeleteAsync(string id);
    }
}
