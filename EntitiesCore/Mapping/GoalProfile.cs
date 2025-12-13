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
    public class GoalProfile : Profile
    {
        public GoalProfile()
        {
            CreateMap<GoalFL, GoalDto>();
            CreateMap<GoalCreateDto, GoalFL>();
            CreateMap<GoalUpdateDto, GoalFL>();
        }
    }
}
