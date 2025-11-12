using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IFolderRepository
    {
        Task<IEnumerable<Folder>> GetAllAsync();
        Task<Folder> GetByIdAsync(string id);
        Task AddAsync(Folder folder);
        Task UpdateAsync(Folder folder);
        Task DeleteAsync(string id);
    }
}
