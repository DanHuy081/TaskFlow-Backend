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
    public class TimeEntryService : ITimeEntryService
    {
        private readonly ITimeEntryRepository _repo;

        public TimeEntryService(ITimeEntryRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<TimeEntryFL>> GetAllAsync() => _repo.GetAllAsync();

        public Task<TimeEntryFL?> GetByIdAsync(string id) => _repo.GetByIdAsync(id);

        public Task<TimeEntryFL> CreateAsync(TimeEntryFL entry)
        {
            entry.DateCreated = DateTime.UtcNow;
            return _repo.CreateAsync(entry);
        }

        public Task<TimeEntryFL> UpdateAsync(TimeEntryFL entry) => _repo.UpdateAsync(entry);

        public Task<bool> DeleteAsync(string id) => _repo.DeleteAsync(id);
    }
}
