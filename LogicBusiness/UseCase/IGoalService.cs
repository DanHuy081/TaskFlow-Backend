using CoreEntities.Model;
using CoreEntities.Model.DTOs;
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
        Task<IEnumerable<GoalDto?>> GetByTeamIdAsync(string teamId);
        Task<GoalFL> GetByIdAsync(string id);
        Task<GoalDto> CreateAsync(GoalCreateDto dto);
        Task<bool> UpdateAsync(string goalId, GoalUpdateDto dto);
        Task<bool> DeleteAsync(string goalId);
    }
}
