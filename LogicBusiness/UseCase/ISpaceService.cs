using CoreEntities.Model;
using CoreEntities.Model.DTOs;
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
        Task<SpaceDto> CreateAsync(SpaceCreateDto dto, string userId);
        Task UpdateAsync(Space space);
        Task DeleteAsync(string id);
        Task<List<SpaceDto>> GetMySpacesAsync(string userId);
    }
}
