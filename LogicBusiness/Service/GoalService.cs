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
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _repo;

        public GoalService(IGoalRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GoalFL>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<GoalFL>> GetByTeamIdAsync(string teamId)
            => await _repo.GetByTeamIdAsync(teamId);

        public async Task<GoalFL> GetByIdAsync(string id)
            => await _repo.GetByIdAsync(id);

        public async Task AddAsync(GoalFL goal)
        {
            goal.GoalId = Guid.NewGuid().ToString();
            goal.DateCreated = DateTime.UtcNow;
            await _repo.AddAsync(goal);
        }

        public async Task UpdateAsync(GoalFL goal)
            => await _repo.UpdateAsync(goal);

        public async Task DeleteAsync(string id)
            => await _repo.DeleteAsync(id);
    }
}
