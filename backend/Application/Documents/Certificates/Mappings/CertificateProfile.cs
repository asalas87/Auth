using Application.Common.Dtos;
using Application.Documents.Certificate.Create;
using Application.Documents.Certificate.DTOs;
using Application.Documents.Certificate.GetAll;
using Application.Documents.Certificate.Update;
using AutoMapper;

namespace Application.Documents.Certificate.Mappings
{
    public class CertificateProfile : Profile
    {
        public CertificateProfile()
        {
            CreateMap<GetCertificatesPaginatedQuery, PaginateDTO>().ReverseMap();
            CreateMap<CertificateDTO, CreateCertificateCommand>()
                .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File));
            CreateMap<CertificateEditDTO, UpdateCertificateCommand>().ReverseMap();
            CreateMap<GetCertificatesPaginatedQuery, CertificateDTO>().ReverseMap();
        }
    }
}
