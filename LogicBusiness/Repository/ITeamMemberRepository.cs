using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface ITeamMemberRepository
    {
        Task<IEnumerable<TeamMember>> GetAllAsync();
        Task<IEnumerable<TeamMember>> GetByTeamIdAsync(string teamId);
        Task<IEnumerable<TeamMember>> GetByUserIdAsync(string userId);
        Task<TeamMember> GetAsync(string teamId, string userId);
        Task AddAsync(TeamMember member);
        Task UpdateAsync(TeamMember member);
        Task DeleteAsync(string teamId, string userId);
    }
}
