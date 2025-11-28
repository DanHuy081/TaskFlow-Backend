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
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            // DTO → Entity
            CreateMap<CommentCreateDto, Comment>();
            CreateMap<CommentUpdateDto, Comment>();

            // Entity → DTO
            CreateMap<Comment, CommentDto>();
        }
    }
}
