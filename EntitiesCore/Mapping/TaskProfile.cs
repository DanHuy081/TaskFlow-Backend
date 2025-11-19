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
        }
    }
}
