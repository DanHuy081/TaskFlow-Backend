using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IListRepository
    {
        Task<IEnumerable<List>> GetAllAsync();
        Task<List> GetByIdAsync(string id);
        Task<List> CreateAsync(List entity);
        Task UpdateAsync(List list);
        Task DeleteAsync(string id);

        Task<List<List>> GetBySpaceIdAsync(string spaceId);
        Task<List<List>> GetByFolderIdAsync(string folderId);

        Task<List<ListBriefDto>> GetListsBySpaceIdAsync(string spaceId);
        Task<List<ListBriefDto>> GetListsByUserIdAsync(string userId);
    }
}
