using CoreEntities.Model;
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

        public CommentService(ICommentRepository repository)
        {
            _repository = repository;
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

        public async Task AddCommentAsync(Comment comment)
        {
            comment.CommentId = Guid.NewGuid().ToString();
            comment.DateCreated = DateTime.UtcNow;
            await _repository.AddAsync(comment);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            comment.DateUpdated = DateTime.UtcNow;
            comment.IsEdited = true;
            await _repository.UpdateAsync(comment);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

