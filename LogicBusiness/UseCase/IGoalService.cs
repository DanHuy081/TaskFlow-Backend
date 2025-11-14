using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface IGoalService
    {
        Task<IEnumerable<GoalFL>> GetAllAsync();
        Task<IEnumerable<GoalFL>> GetByTeamIdAsync(string teamId);
        Task<GoalFL> GetByIdAsync(string id);
        Task AddAsync(GoalFL goal);
        Task UpdateAsync(GoalFL goal);
        Task DeleteAsync(string id);
    }
}
