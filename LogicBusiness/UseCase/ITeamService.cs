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
        Task DeleteAsync(string id);
        Task<Team> CreateTeamAsync(CreateTeamDto dto, string userId);
    }
}
