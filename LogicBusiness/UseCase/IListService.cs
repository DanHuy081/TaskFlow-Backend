using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface IListService
    {
        Task<IEnumerable<ListFL>> GetAllAsync();
        Task<ListFL> GetByIdAsync(string id);
        Task AddAsync(ListFL list);
        Task UpdateAsync(ListFL list);
        Task DeleteAsync(string id);
    }
}
