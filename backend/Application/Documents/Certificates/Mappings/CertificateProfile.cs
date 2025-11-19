using Application.Common.Dtos;
using Application.Documents.Certificate.Create;
using Application.Documents.Certificate.Dtos;
using Application.Documents.Certificate.GetAll;
using Application.Documents.Certificate.Update;
using Application.Documents.Renovation.Dtos;
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
            CreateMap<RenovationEditDTO, UpdateCertificateCommand>().ReverseMap();
            CreateMap<GetCertificatesPaginatedQuery, CertificateDTO>().ReverseMap();
        }
    }
}
