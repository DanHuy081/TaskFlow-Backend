using CoreEntities.Model;
using CoreEntities.Model.DTOs;
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
        Task<ListDto> CreateAsync(ListCreateDto dto);
        Task<List<ListDto>> GetBySpaceAsync(string spaceId);
        Task<List<ListDto>> GetByFolderAsync(string folderId);
        Task UpdateAsync(List list);
        Task DeleteAsync(string id);

        //-----
        Task<List<ListBriefDto>> GetListsByUserIdAsync(string userId);
        Task<List<ListBriefDto>> GetListsBySpaceIdAsync(string spaceId);
    }
}
