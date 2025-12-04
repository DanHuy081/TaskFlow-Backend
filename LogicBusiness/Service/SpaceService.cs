using AutoMapper;
using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class SpaceService : ISpaceService
    {
        private readonly ISpaceRepository _repo;
        private readonly IMapper _mapper;


        public SpaceService(ISpaceRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Space>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Space> GetByIdAsync(string id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<SpaceDto> CreateAsync(SpaceCreateDto dto, string userId)
        {
            var space = new Space
            {
                SpaceId = Guid.NewGuid().ToString(),
                TeamId = dto.TeamId,
                Name = dto.Name,
                Color = "#7A08FA",       // default optional
                IsPrivate = false,
                Settings = null,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            await _repo.CreateAsync(space);

            return _mapper.Map<SpaceDto>(space);
        }

        public async Task UpdateAsync(Space space)
        {
            space.DateUpdated = DateTime.UtcNow;
            await _repo.UpdateAsync(space);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<List<SpaceDto>> GetMySpacesAsync(string userId)
        {
            var spaces = await _repo.GetSpacesByUserAsync(userId);

            return _mapper.Map<List<SpaceDto>>(spaces);
        }
    }
}
