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
    public class ChecklistService : IChecklistService
    {
        private readonly IChecklistRepository _repo;

        public ChecklistService(IChecklistRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ChecklistFL>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<ChecklistFL>> GetByTaskIdAsync(string taskId)
            => await _repo.GetByTaskIdAsync(taskId);

        public async Task<ChecklistFL> GetByIdAsync(string id)
            => await _repo.GetByIdAsync(id);

        public async Task AddAsync(ChecklistFL checklist)
        {
            checklist.ChecklistId = Guid.NewGuid().ToString();
            checklist.DateCreated = DateTime.UtcNow;
            await _repo.AddAsync(checklist);
        }

        public async Task UpdateAsync(ChecklistFL checklist)
            => await _repo.UpdateAsync(checklist);

        public async Task DeleteAsync(string id)
            => await _repo.DeleteAsync(id);
    }
}
