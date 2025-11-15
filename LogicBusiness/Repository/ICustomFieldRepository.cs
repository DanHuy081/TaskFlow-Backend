using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface ICustomFieldRepository
    {
        Task<IEnumerable<CustomFieldFL>> GetAllAsync();
        Task<CustomFieldFL?> GetByIdAsync(string id);
        Task<CustomFieldFL> CreateAsync(CustomFieldFL entity);
        Task<CustomFieldFL> UpdateAsync(CustomFieldFL entity);
        Task<bool> DeleteAsync(string id);
    }
}
