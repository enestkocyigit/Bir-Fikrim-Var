using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BirFikrimVar.Entity.DTOs;
using BirFikrimVar.Entity.Models;

namespace BirFikrimVar.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<UserRegisterDto, User>();

            CreateMap<IdeaCreateDto, Idea>()
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.Ignore());

            CreateMap<CommentCreateDto, Comment>()
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.Ignore());

            CreateMap<IdeaEditDto, Idea>()
    .           ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore());
        }
    }
}
