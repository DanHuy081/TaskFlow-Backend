using AutoMapper;
using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Mapping
{
    public class ChecklistItemsProfile : Profile
    {
        public ChecklistItemsProfile()
        {
            CreateMap<ChecklistItemsProfile,  ChecklistItemFL>();
            CreateMap<ChecklistItemFL, ChecklistItemDto>();
        }
    }
}
