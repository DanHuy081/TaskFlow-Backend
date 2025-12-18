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
    public class ListService : IListService
    {
        private readonly IListRepository _repo;
        private readonly IMapper _mapper;

        public ListService(IListRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<List>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<List> GetByIdAsync(string id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<ListDto> CreateAsync(ListCreateDto dto)
        {
            var entity = new List
            {
                ListId = Guid.NewGuid().ToString(),
                SpaceId = dto.SpaceId,
                FolderId = dto.FolderId,
                Name = dto.Name,
                Status = dto.Status,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            await _repo.CreateAsync(entity);

            return _mapper.Map<ListDto>(entity);
        }

        public async Task UpdateAsync(List list)
        {
            list.DateUpdated = DateTime.UtcNow;
            await _repo.UpdateAsync(list);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<List<ListDto>> GetBySpaceAsync(string spaceId)
        {
            var data = await _repo.GetBySpaceIdAsync(spaceId);
            return _mapper.Map<List<ListDto>>(data);
        }

        public async Task<List<ListDto>> GetByFolderAsync(string folderId)
        {
            var data = await _repo.GetByFolderIdAsync(folderId);
            return _mapper.Map<List<ListDto>>(data);
        }

        public Task<List<ListBriefDto>> GetListsByUserIdAsync(string userId)
            => _repo.GetListsByUserIdAsync(userId);

        public Task<List<ListBriefDto>> GetListsBySpaceIdAsync(string spaceId)
            => _repo.GetListsBySpaceIdAsync(spaceId);
    }
}
