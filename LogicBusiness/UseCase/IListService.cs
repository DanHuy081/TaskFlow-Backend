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
        Task<IEnumerable<List>> GetAllAsync();
        Task<List> GetByIdAsync(string id);
        Task AddAsync(List list);
        Task UpdateAsync(List list);
        Task DeleteAsync(string id);
    }
}
