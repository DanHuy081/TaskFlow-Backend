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
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _repo;

        public TeamService(ITeamRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Team> GetByIdAsync(string id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(Team team)
        {
            team.TeamId = Guid.NewGuid().ToString();
            team.DateCreated = DateTime.UtcNow;
            await _repo.AddAsync(team);
        }

        public async Task UpdateAsync(Team team)
        {
            team.DateUpdated = DateTime.UtcNow;
            await _repo.UpdateAsync(team);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
