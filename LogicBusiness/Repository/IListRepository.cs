using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IListRepository
    {
        Task<IEnumerable<ListFL>> GetAllAsync();
        Task<ListFL> GetByIdAsync(string id);
        Task AddAsync(ListFL list);
        Task UpdateAsync(ListFL list);
        Task DeleteAsync(string id);
    }
}
