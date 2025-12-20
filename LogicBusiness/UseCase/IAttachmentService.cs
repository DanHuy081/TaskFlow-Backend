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
        Task<string> UploadAsync(IFormFile file, string folderName);
        Task DeleteAsync(string id, string rootPath);
    }
}
