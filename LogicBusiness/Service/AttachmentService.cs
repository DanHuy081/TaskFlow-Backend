using CoreEntities.Model;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
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
        private readonly IWebHostEnvironment _environment;

        public AttachmentService(IAttachmentRepository repo, IWebHostEnvironment environment)
        {
            _repo = repo;
            _environment = environment;
        }

        public async Task<IEnumerable<Attachment>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<IEnumerable<Attachment>> GetByTaskIdAsync(string taskId) => await _repo.GetByTaskIdAsync(taskId);

        public async Task<IEnumerable<Attachment>> GetByCommentIdAsync(string commentId) => await _repo.GetByCommentIdAsync(commentId);

        public async Task<Attachment> GetByIdAsync(string id) => await _repo.GetByIdAsync(id);

        public async Task<string> UploadAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ");

            // 1. Tạo đường dẫn thư mục lưu: wwwroot/uploads/tasks
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folderName);

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // 2. Tạo tên file duy nhất (dùng Guid)
            // Ví dụ: avatar.png -> 3f8a...2b1c_avatar.png
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            // 3. Đường dẫn vật lý đầy đủ
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // 4. Lưu file vào ổ cứng
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // 5. Trả về đường dẫn tương đối để Client truy cập được (Ví dụ: /uploads/tasks/abc.png)
            return $"/uploads/{folderName}/{uniqueFileName}";
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
