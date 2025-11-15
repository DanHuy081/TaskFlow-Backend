using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface ITimeEntryRepository
    {
        Task<IEnumerable<TimeEntryFL>> GetAllAsync();
        Task<TimeEntryFL?> GetByIdAsync(string id);
        Task<TimeEntryFL> CreateAsync(TimeEntryFL entry);
        Task<TimeEntryFL> UpdateAsync(TimeEntryFL entry);
        Task<bool> DeleteAsync(string id);
    }
}
