using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetCommentsByTaskIdAsync(int taskId);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<Comment> AddCommentAsync(Comment comment);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int id);
    }
}
