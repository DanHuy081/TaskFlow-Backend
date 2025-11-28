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
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByTaskIdAsync(string taskId)
        {
            return await _repository.GetByTaskIdAsync(taskId);
        }

        public async Task<CommentDto> CreateAsync(CommentCreateDto dto)
        {
            var entity = _mapper.Map<Comment>(dto);
            entity.CommentId = Guid.NewGuid().ToString();
            entity.IsEdited = false;
            entity.DateCreated = DateTime.UtcNow;
            entity.DateUpdated = DateTime.UtcNow;

            await _repository.CreateAsync(entity);

            return _mapper.Map<CommentDto>(entity);
        }

        public async Task<CommentDto?> UpdateAsync(string id, CommentUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;


            _mapper.Map(dto, entity);
            entity.IsEdited = true;
            entity.DateUpdated = DateTime.UtcNow;


            await _repository.UpdateAsync(entity);
            return _mapper.Map<CommentDto>(entity);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

