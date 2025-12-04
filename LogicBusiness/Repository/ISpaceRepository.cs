using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface ISpaceRepository
    {
        Task<IEnumerable<Space>> GetAllAsync();
        Task<Space> GetByIdAsync(string id);
        Task CreateAsync(Space space);
        Task UpdateAsync(Space space);
        Task DeleteAsync(string id);
        Task<List<Space>> GetSpacesByUserAsync(string userId, string? teamId = null);
    }
}
