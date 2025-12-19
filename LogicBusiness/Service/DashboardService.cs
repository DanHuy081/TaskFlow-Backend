using CoreEntities.Model.DTOs;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;

        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<DashboardDto> GetMyDashboardAsync(string userId, Guid? teamId)
        {
            // Có thể thêm logic check xem User có thuộc TeamId này không ở đây
            return await _repo.GetDashboardStatisticsAsync(userId, teamId);
        }
    }
}
