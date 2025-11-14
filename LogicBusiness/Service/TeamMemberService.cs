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
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _repo;

        public TeamMemberService(ITeamMemberRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TeamMember>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<TeamMember>> GetByTeamIdAsync(string teamId)
            => await _repo.GetByTeamIdAsync(teamId);

        public async Task<IEnumerable<TeamMember>> GetByUserIdAsync(string userId)
            => await _repo.GetByUserIdAsync(userId);

        public async Task<TeamMember> GetAsync(string teamId, string userId)
            => await _repo.GetAsync(teamId, userId);

        public async Task AddAsync(TeamMember member)
        {
            member.DateJoined = DateTime.UtcNow;
            await _repo.AddAsync(member);
        }

        public async Task UpdateAsync(TeamMember member)
            => await _repo.UpdateAsync(member);

        public async Task DeleteAsync(string teamId, string userId)
            => await _repo.DeleteAsync(teamId, userId);
    }
}
