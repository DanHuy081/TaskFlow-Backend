using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface ICalendarService
    {
        Task<List<CalendarTaskDto>> GetMyCalendarAsync(
            string userId,
            DateTime from,
            DateTime to
        );

        Task<bool> MoveTaskAsync(string taskId, MoveTaskCalendarDto dto);
    }

}
