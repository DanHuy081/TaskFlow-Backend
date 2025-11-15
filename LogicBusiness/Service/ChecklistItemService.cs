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
    public class ChecklistItemService : IChecklistItemService
    {
        private readonly IChecklistItemRepository _repo;

        public ChecklistItemService(IChecklistItemRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ChecklistItemFL>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<ChecklistItemFL>> GetByChecklistIdAsync(string checklistId)
            => await _repo.GetByChecklistIdAsync(checklistId);

        public async Task<ChecklistItemFL> GetByIdAsync(string id)
            => await _repo.GetByIdAsync(id);

        public async Task AddAsync(ChecklistItemFL item)
        {
            item.ChecklistItemId = Guid.NewGuid().ToString();
            item.DateCreated = DateTime.UtcNow;
            await _repo.AddAsync(item);
        }

        public async Task UpdateAsync(ChecklistItemFL item)
            => await _repo.UpdateAsync(item);

        public async Task DeleteAsync(string id)
            => await _repo.DeleteAsync(id);
    }
}
