using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IGoalRepository
    {
        Task<IEnumerable<GoalFL>> GetAllAsync();
        Task<IEnumerable<GoalFL>> GetByTeamIdAsync(string teamId);
        Task<GoalFL> GetByIdAsync(string id);
        Task AddAsync(GoalFL goal);
        Task UpdateAsync(GoalFL goal);
        Task DeleteAsync(GoalFL goal);
    }
}
