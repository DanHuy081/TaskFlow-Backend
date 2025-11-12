using CoreEntities.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface IAttachmentService
    {
        Task<IEnumerable<Attachment>> GetAllAsync();
        Task<IEnumerable<Attachment>> GetByTaskIdAsync(string taskId);
        Task<IEnumerable<Attachment>> GetByCommentIdAsync(string commentId);
        Task<Attachment> GetByIdAsync(string id);
        Task<Attachment> UploadAsync(IFormFile file, string? taskId, string? commentId, string uploadedBy, string rootPath);
        Task DeleteAsync(string id, string rootPath);
    }
}
