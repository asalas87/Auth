using Application.Common.Dtos;
using Application.Documents.Renovation.Create;
using Application.Documents.Renovation.Dtos;
using Application.Documents.Renovation.DTOs;
using Application.Documents.Renovation.GetAll;
using Application.Documents.Renovation.Update;
using AutoMapper;

namespace Application.Documents.Renovations.Mappings;

public class RenovationProfile : Profile
{
    public RenovationProfile()
    {
        CreateMap<GetRenovationsPaginatedQuery, PaginateDTO>().ReverseMap();
        CreateMap<RenovationDTO, CreateRenovationCommand>()
            .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File));
        CreateMap<RenovationEditDTO, UpdateRenovationCommand>().ReverseMap();
        CreateMap<GetRenovationsPaginatedQuery, RenovationDTO>().ReverseMap();
    }
}
