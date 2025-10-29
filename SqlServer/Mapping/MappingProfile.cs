using AutoMapper;
using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Tự động map giữa Entity và DTO
            CreateMap<TaskFL, TaskDto>().ReverseMap();
        }
    }
}
