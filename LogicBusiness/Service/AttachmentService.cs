using CoreEntities.Model;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _repo;

        public AttachmentService(IAttachmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Attachment>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<IEnumerable<Attachment>> GetByTaskIdAsync(string taskId) => await _repo.GetByTaskIdAsync(taskId);

        public async Task<IEnumerable<Attachment>> GetByCommentIdAsync(string commentId) => await _repo.GetByCommentIdAsync(commentId);

        public async Task<Attachment> GetByIdAsync(string id) => await _repo.GetByIdAsync(id);

        public async Task<Attachment> UploadAsync(IFormFile file, string? taskId, string? commentId, string uploadedBy, string rootPath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ.");

            string folder = Path.Combine(rootPath, "uploads", taskId ?? commentId ?? "misc");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(folder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string relativeUrl = Path.Combine("uploads", taskId ?? commentId ?? "misc", uniqueFileName).Replace("\\", "/");

            var attachment = new Attachment
            {
                AttachmentId = Guid.NewGuid().ToString(),
                TaskId = taskId,
                CommentId = commentId,
                FileName = file.FileName,
                Url = relativeUrl,
                SizeBytes = file.Length,
                MimeType = file.ContentType,
                UploadedBy = uploadedBy,
                DateAdded = DateTime.UtcNow
            };

            await _repo.AddAsync(attachment);
            return attachment;
        }

        public async Task DeleteAsync(string id, string rootPath)
        {
            var file = await _repo.GetByIdAsync(id);
            if (file == null) return;

            var fullPath = Path.Combine(rootPath, file.Url);
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            await _repo.DeleteAsync(id);
        }
    }
}
