﻿using AutoMapper;
using BlogAPI.Models;
using BlogAPI.Models.Dto;

namespace BlogAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Post, PostDTO>().ReverseMap();
            CreateMap<Post, PostCreateDTO>().ReverseMap();
            CreateMap<Post, PostUpdateDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserCreateDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();

            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Comment, CommentCreateDTO>().ReverseMap();
            CreateMap<Comment, CommentUpdateDTO>().ReverseMap();
        }
    }
}
