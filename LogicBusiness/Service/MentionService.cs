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
    
    public class MentionService : IMentionService
    {
        private readonly IUserRepository _userRepo;
        private readonly ITaskRepository _taskRepo;

        public MentionService(IUserRepository userRepo, ITaskRepository taskRepo)
        {
            _userRepo = userRepo;
            _taskRepo = taskRepo;
        }

        public async Task<List<MentionSuggestionDto>> SearchAsync(string keyword)
        {
            var result = new List<MentionSuggestionDto>();

            // 1. Lấy User
            var users = await _userRepo.SearchUsersAsync(keyword);
            result.AddRange(users.Select(u => new MentionSuggestionDto
            {
                Id = u.UserId,
                DisplayText = u.FullName,
                Type = "User",
                
            }));

            // 2. Lấy Task
            var tasks = await _taskRepo.SearchTasksAsync(keyword);
            result.AddRange(tasks.Select(t => new MentionSuggestionDto
            {
                Id = t.Id,
                DisplayText = t.Name,
                Type = "Task"
            }));

            return result;
        }
    }
}
