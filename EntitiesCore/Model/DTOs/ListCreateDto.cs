using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class ListCreateDto
    {
        public string? SpaceId { get; set; }   // hoặc null nếu tạo trong Folder
        public string? FolderId { get; set; }  // hoặc null nếu tạo trực tiếp dưới Space
        public string Name { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }

}
