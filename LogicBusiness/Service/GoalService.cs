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
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _repo;
        private readonly IMapper _mapper;

        public GoalService(IGoalRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GoalFL>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<GoalDto>> GetByTeamIdAsync(string teamId)
        {
            var goals = await _repo.GetByTeamIdAsync(teamId);
            return _mapper.Map<List<GoalDto>>(goals);
        } 
        public async Task<GoalFL> GetByIdAsync(string id)
            => await _repo.GetByIdAsync(id);

        public async Task<GoalDto> CreateAsync(GoalCreateDto dto)
        {
            var goal = _mapper.Map<GoalFL>(dto);
            goal.GoalId = Guid.NewGuid().ToString();
            goal.DateCreated = DateTime.UtcNow;
            goal.Progress = 0;

            await _repo.UpdateAsync(goal);

            return _mapper.Map<GoalDto>(goal);
        }

        public async Task<bool> UpdateAsync(string goalId, GoalUpdateDto dto)
        {
            var goal = await _repo.GetByIdAsync(goalId);
            if (goal == null) return false;

            _mapper.Map(dto, goal);
            await _repo.UpdateAsync(goal);

            return true;
        }

        public async Task<bool> DeleteAsync(string goalId)
        {
            var goal = await _repo.GetByIdAsync(goalId);
            if (goal == null) return false;

            await _repo.DeleteAsync(goal);
            return true;
        }
    }
}
