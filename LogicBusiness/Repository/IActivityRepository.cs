using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IActivityRepository
    {
        Task AddLogAsync(ActivityLog log);
        Task<List<ActivityLog>> GetLogsByTeamIdAsync(Guid teamId);
    }
}
