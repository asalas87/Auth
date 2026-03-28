using Application.Common.Dtos;
using Application.Documents.Common.DTOs;
using Application.Documents.Management.Create;
using Application.Documents.Management.DTOs;
using Application.Documents.Management.GetAll;
using AutoMapper;
using Domain.Documents.Entities;

namespace Application.Documents.Management.Mappings;
public class DocumentProfile : Profile
{
    public DocumentProfile()
    {
        CreateMap<GetDocumentsPaginatedQuery, PaginateDTO>().ReverseMap();
        CreateMap<GetDocumentsPaginatedByAssignedToQuery, DocumentAssignedDTO>().ReverseMap();
        CreateMap<DocumentDTO, CreateDocumentCommand>()
            .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File));
        CreateMap<GetDocumentsPaginatedQuery, DocumentResponseDTO>().ReverseMap();

        CreateMap<DocumentFile, DocumentGridResponseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.IsRead))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "Documento"));
        CreateMap<Domain.Documents.Entities.Certificate, DocumentGridResponseDTO>()
            .IncludeBase<DocumentFile, DocumentGridResponseDTO>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "Certificado"))
            .ForMember(dest => dest.CertificateNumber, opt => opt.MapFrom(src => src.CertificateNumber))
            .ForMember(dest => dest.EmployerFullName, opt => opt.MapFrom(src => src.EmployerFullName))
            .ForMember(dest => dest.StandardCode, opt => opt.MapFrom(src => src.StandardCode))
            .ForMember(dest => dest.Validity, opt => opt.MapFrom(src => src.ExpirationDate));
        CreateMap<Domain.Documents.Entities.Renovation, DocumentGridResponseDTO>()
            .IncludeBase<Domain.Documents.Entities.Certificate, DocumentGridResponseDTO>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "Renovación"));
    }
}
