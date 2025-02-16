using AutoMapper;
using BlogWeb.Models.Dto;

namespace BlogWeb
{
    public class MappingConfig : Profile
    {
        public MappingConfig() { 
        
            CreateMap<PostDTO, PostCreateDTO>().ReverseMap();
        }
    }
}
