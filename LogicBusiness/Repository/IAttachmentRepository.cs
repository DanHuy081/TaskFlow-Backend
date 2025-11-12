using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IAttachmentRepository
    {
        Task<IEnumerable<Attachment>> GetAllAsync();
        Task<IEnumerable<Attachment>> GetByTaskIdAsync(string taskId);
        Task<IEnumerable<Attachment>> GetByCommentIdAsync(string commentId);
        Task<Attachment> GetByIdAsync(string id);
        Task AddAsync(Attachment attachment);
        Task DeleteAsync(string id);
    }
}
