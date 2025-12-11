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
    public class ChecklistItemService : IChecklistItemService
    {
        private readonly IChecklistItemRepository _repo;
        private readonly IMapper _mapper;

        public ChecklistItemService(IChecklistItemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChecklistItemFL>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<ChecklistItemDto>> GetByChecklistIdAsync(string checklistId)
        {
            var list = await _repo.GetByChecklistIdAsync(checklistId);
            return _mapper.Map<IEnumerable<ChecklistItemDto>>(list);
        }


        public async Task<ChecklistItemFL> GetByIdAsync(string id)
            => await _repo.GetByIdAsync(id);

        public async Task<ChecklistItemDto> CreateAsync(CreateChecklistItemDto dto)
        {
            var item = new ChecklistItemFL
            {
                ChecklistItemId = Guid.NewGuid().ToString(),
                ChecklistId = dto.ChecklistId,
                Name = dto.Name,
                IsResolved = false,
                OrderIndex = 0,
                DateCreated = DateTime.UtcNow
            };

            await _repo.CreateAsync(item);
            return _mapper.Map<ChecklistItemDto>(item);
        }

        public async Task UpdateAsync(ChecklistItemFL item)
            => await _repo.UpdateAsync(item);

        public async Task DeleteAsync(string id)
            => await _repo.DeleteAsync(id);

        public async Task ToggleResolvedAsync(string itemId, string userId)
        {
            await _repo.ToggleResolvedAsync(itemId, userId);
        }
    }
}
