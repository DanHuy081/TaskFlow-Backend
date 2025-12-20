using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Space = CoreEntities.Model.Space;

namespace LogicBusiness.UseCase
{
    public interface ISpaceService
    {
        Task<IEnumerable<Space>> GetAllAsync();
        Task<Space> GetByIdAsync(string id);
        Task<SpaceDto> CreateAsync(SpaceCreateDto dto, string userId);
        Task UpdateAsync(Space space);
        Task DeleteSpaceCascadeAsync(string spaceId);
        Task<List<SpaceDto>> GetMySpacesAsync(string userId);
        Task<List<SpaceBriefDto>> GetSpacesByUserIdAsync(string userId);
        Task<List<SpaceBriefDto>> GetSpacesByTeamIdAsync(string teamId);
        Task<SpaceBriefDto?> GetSpaceByIdAsync(string spaceId);
        Task<PersonalSpaceDto> GetPersonalWorkspaceAsync(string userId);
    }
}
