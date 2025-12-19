using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface IDashboardService
    {
       Task<DashboardDto> GetMyDashboardAsync(string userId, Guid? teamId);
    }
}
