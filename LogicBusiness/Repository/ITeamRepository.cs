using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team> GetByIdAsync(string id);
        Task AddAsync(Team team);
        Task UpdateAsync(Team team);
        Task DeleteAsync(string id);
        //
        Task AddTeamAsync(Team team);
        Task AddTeamMemberAsync(TeamMember member);
    }
}
