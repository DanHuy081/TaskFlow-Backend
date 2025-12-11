using AutoMapper;
using CoreEntities.Model.DTOs;
using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Mapping
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            // Map từ CreateDto sang Entity
            CreateMap<TaskCreateDto, TaskFL>();

            // Map từ Entity sang TaskDto để trả về client
            CreateMap<TaskFL, TaskDto>();

            CreateMap<TaskStatusUpdateDto, TaskFL>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<TaskUpdateDto, TaskFL>()
            // Dòng dưới giúp bỏ qua các giá trị null khi update.
            // Nghĩa là: Nếu DTO gửi field nào là null, nó sẽ GIỮ NGUYÊN giá trị cũ trong database, không bị ghi đè thành null.
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));             
        }
    }
}
