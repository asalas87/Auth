using Application.Common.Dtos;
using Application.Documents.Management.Create;
using Application.Documents.Management.DTOs;
using Application.Documents.Management.GetAll;
using AutoMapper;

namespace Application.Documents.Management.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<GetDocumentsPaginatedQuery, PaginateDTO>().ReverseMap();
            CreateMap<CreateDocumentDTO, CreateDocumentCommand>()
                .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File));
            CreateMap<GetDocumentsPaginatedQuery, DocumentFileDTO>().ReverseMap();
        }
    }
}
