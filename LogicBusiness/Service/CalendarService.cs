using AutoMapper;
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
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepository _repo;
        private readonly IMapper _mapper;

        public CalendarService(ICalendarRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CalendarTaskDto>> GetMyCalendarAsync(
            string userId,
            DateTime from,
            DateTime to)
        {
            var tasks = await _repo.GetTasksForCalendarAsync(userId, from, to);

            return tasks.Select(t => new CalendarTaskDto
            {
                Id = t.Id,
                Title = string.IsNullOrEmpty(t.Name) ? "Untitled Task" : t.Name,
                Start = t.StartDate,
                End = t.DueDate,
                Status = t.Status
            }).ToList();
        }

        public async Task<bool> MoveTaskAsync(string taskId, MoveTaskCalendarDto dto)
        {
            var task = await _repo.GetTaskByIdAsync(taskId);
            if (task == null) throw new Exception("Task not found");

            task.StartDate = dto.StartDate;
            task.DueDate = dto.DueDate;
            task.DateUpdated = DateTime.UtcNow;

            await _repo.UpdateTaskAsync(task);
            return true;
        }
    }

}
