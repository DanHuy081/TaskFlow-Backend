using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment> GetCommentByIdAsync(string id);
        Task<IEnumerable<Comment>> GetCommentsByTaskIdAsync(string taskId);
        Task<CommentDto> CreateAsync(CommentCreateDto dto);
        Task<CommentDto?> UpdateAsync(string id, CommentUpdateDto dto);
        Task DeleteCommentAsync(string id);
    }
}
