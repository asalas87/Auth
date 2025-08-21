using Application.Common.Dtos;
using Application.Documents.Common.DTOs;
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
            CreateMap<GetDocumentsPaginatedByAssignedToQuery, DocumentAssignedDTO>().ReverseMap();
            CreateMap<DocumentDTO, CreateDocumentCommand>()
                .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File));
            CreateMap<GetDocumentsPaginatedQuery, DocumentResponseDTO>().ReverseMap();
        }
    }
}
