using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface ISpaceService
    {
        Task<IEnumerable<Space>> GetAllAsync();
        Task<Space> GetByIdAsync(string id);
        Task AddAsync(Space space);
        Task UpdateAsync(Space space);
        Task DeleteAsync(string id);
    }
}
