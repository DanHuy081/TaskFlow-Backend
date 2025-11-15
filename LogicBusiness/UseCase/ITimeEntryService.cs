using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface ITimeEntryService
    {
        Task<IEnumerable<TimeEntryFL>> GetAllAsync();
        Task<TimeEntryFL?> GetByIdAsync(string id);
        Task<TimeEntryFL> CreateAsync(TimeEntryFL entry);
        Task<TimeEntryFL> UpdateAsync(TimeEntryFL entry);
        Task<bool> DeleteAsync(string id);
    }
}
