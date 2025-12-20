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
    public class SpaceMappingProfile : Profile
    {
        public SpaceMappingProfile()
        {
            CreateMap<Model.Space, SpaceDto>();
        }
    }
}
