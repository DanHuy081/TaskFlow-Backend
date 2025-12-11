using AutoMapper;
using CoreEntities.Model;
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
    public class ChecklistService : IChecklistService
    {
        private readonly IChecklistRepository _repo;
        private readonly IMapper _mapper;

        public ChecklistService(IChecklistRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChecklistFL>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<ChecklistDto>> GetByTaskIdAsync(string taskId)
        {
            var list = await _repo.GetByTaskIdAsync(taskId);
            return _mapper.Map<IEnumerable<ChecklistDto>>(list);
        }

        public async Task<ChecklistFL> GetByIdAsync(string id)
            => await _repo.GetByIdAsync(id);

        public async Task<ChecklistDto> CreateAsync(CreateChecklistDto dto)
        {
            var checklist = new ChecklistFL
            {
                ChecklistId = Guid.NewGuid().ToString(),
                TaskId = dto.TaskId,
                Name = dto.Name,
                DateCreated = DateTime.UtcNow
            };

            await _repo.CreateAsync(checklist);
            return _mapper.Map<ChecklistDto>(checklist);
        }

        public async Task UpdateAsync(ChecklistFL checklist)
            => await _repo.UpdateAsync(checklist);

        public async Task DeleteAsync(string id)
            => await _repo.DeleteAsync(id);
    }
}
