using CoreEntities.Model;
using CoreEntities.Model.DTOs;
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
        Task DeleteSpaceCascadeAsync(string spaceId);

        Task<List<Space>> GetSpacesByUserAsync(string userId, string? teamId = null);

        Task<List<SpaceBriefDto>> GetSpacesByUserIdAsync(string userId);
        Task<List<SpaceBriefDto>> GetSpacesByTeamIdAsync(string teamId);
        Task<SpaceBriefDto?> GetSpaceByIdAsync(string spaceId);
    }
}
