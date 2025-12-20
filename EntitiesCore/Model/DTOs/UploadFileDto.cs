using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class UploadFileDto
    {
        public IFormFile File { get; set; }
        public string? TaskId { get; set; }
        public string? CommentId { get; set; }
        public string UploadedBy { get; set; }
    }
}
