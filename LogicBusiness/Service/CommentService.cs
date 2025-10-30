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

        public Task<IEnumerable<Comment>> GetCommentsByTaskIdAsync(int taskId)
            => _repository.GetCommentsByTaskIdAsync(taskId);

        public Task<Comment?> GetCommentByIdAsync(int id)
            => _repository.GetCommentByIdAsync(id);

        public Task<Comment> AddCommentAsync(Comment comment)
            => _repository.AddCommentAsync(comment);

        public Task<Comment> UpdateCommentAsync(Comment comment)
            => _repository.UpdateCommentAsync(comment);

        public Task<bool> DeleteCommentAsync(int id)
            => _repository.DeleteCommentAsync(id);
    }
}

