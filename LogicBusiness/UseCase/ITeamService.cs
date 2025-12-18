using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team> GetByIdAsync(string id);
        Task AddAsync(Team team);
        Task UpdateAsync(Team team);
        Task DeleteTeamCascadeAsync(string teamId);
        Task<Team> CreateTeamAsync(CreateTeamDto dto, string userId);
        Task<bool> AddUserToTeamAsync(string teamId, string email, string role);
        //AI hiểu
        Task<List<TeamBriefDto>> GetTeamsByUserIdAsync(string userId);

    }
}
