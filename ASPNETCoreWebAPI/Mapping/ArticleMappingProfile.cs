using ASPNETCoreWebAPI.Contracts;
using ASPNETCoreWebAPI.Entities;
using AutoMapper;

namespace ASPNETCoreWebAPI.Mapping;

public class ArticleMappingProfile : Profile
{
    public ArticleMappingProfile()
    {
        CreateMap<Article, ArticleResponse>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
           .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => string.Join(", ", src.Authors.Select(a => a.Name)) ?? "(anonymous)"))
           .ForMember(dest => dest.SiteId, opt => opt.MapFrom(src => src.Site.Id))
           .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created))
           .ForMember(dest => dest.Modified, opt => opt.MapFrom(src => src.Modified))
           ;
    }
}
