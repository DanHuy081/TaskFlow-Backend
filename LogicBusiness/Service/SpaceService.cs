using CoreEntities.Model;
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

        public SpaceService(ISpaceRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Space>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Space> GetByIdAsync(string id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(Space space)
        {
            space.SpaceId = Guid.NewGuid().ToString();
            space.DateCreated = DateTime.UtcNow;
            await _repo.AddAsync(space);
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
    }
}
