using CoreEntities.Model;
using LogicBusiness.Repository;
using LogicBusiness.UseCase; // Chứa IAttachmentService
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
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

        // --- ĐÃ SỬA: Cập nhật tham số để nhận đủ thông tin lưu DB ---
        public async Task<Attachment> UploadAsync(IFormFile file, string? taskId, string? commentId, string uploadedBy, string rootPathFromController = null)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ");

            // 1. KHẮC PHỤC LỖI "Path1 cannot be null"
            // Ưu tiên dùng WebRootPath (wwwroot), nếu null thì dùng ContentRootPath, hoặc dùng path truyền từ controller
            string webRoot = _environment.WebRootPath ?? _environment.ContentRootPath ?? rootPathFromController;

            if (string.IsNullOrEmpty(webRoot))
            {
                throw new Exception("Server không xác định được thư mục gốc để lưu file.");
            }

            string folderName = "attachments"; // Đặt tên thư mục chung
            string uploadsFolder = Path.Combine(webRoot, "uploads", folderName);

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // 2. Tạo tên file duy nhất
            string fileExtension = Path.GetExtension(file.FileName);
            string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            // 3. Lưu file vật lý vào ổ cứng
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // 4. Tạo đường dẫn tương đối để trả về FE
            string relativeUrl = $"/uploads/{folderName}/{uniqueFileName}";

            // 5. QUAN TRỌNG: Lưu thông tin vào Database
            var attachment = new Attachment
            {
                AttachmentId = Guid.NewGuid().ToString(),
                FileName = file.FileName,
                Url = relativeUrl,

                // Các trường bổ sung bạn có:
                MimeType = file.ContentType, // Ví dụ: image/jpeg, application/pdf
                SizeBytes = file.Length,     // Kích thước file (bytes)
                DateAdded = DateTime.Now,    // Thay vì DateCreated

                UploadedBy = uploadedBy,
                TaskId = taskId,
                CommentId = commentId
            };

            // Gọi Repository để INSERT vào SQL
            await _repo.AddAsync(attachment);

            return attachment; // Trả về object đầy đủ thay vì chỉ string
        }

        public async Task DeleteAsync(string id)
        {
            // 1. Lấy thông tin file từ DB
            var file = await _repo.GetByIdAsync(id);
            if (file == null) return;

            // 2. Xóa file vật lý trên ổ cứng
            try
            {
                string webRoot = _environment.WebRootPath ?? _environment.ContentRootPath;
                if (!string.IsNullOrEmpty(webRoot) && !string.IsNullOrEmpty(file.Url))
                {
                    // file.Url thường dạng "/uploads/..." nên cần xóa dấu "/" ở đầu để Combine đúng
                    string relativePath = file.Url.TrimStart('/', '\\');
                    string fullPath = Path.Combine(webRoot, relativePath);

                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi xóa file vật lý (không nên chặn xóa DB nếu file vật lý lỗi)
                Console.WriteLine($"Lỗi xóa file vật lý: {ex.Message}");
            }

            // 3. Xóa bản ghi trong Database
            await _repo.DeleteAsync(id);
        }
    }
}