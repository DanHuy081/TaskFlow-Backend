using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IDashboardRepository
    {
        Task<DashboardDto> GetDashboardStatisticsAsync(string userId, Guid? teamId);
    }
}
