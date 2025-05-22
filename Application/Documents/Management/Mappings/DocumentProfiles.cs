using Application.Common.Dtos;
using Application.Documents.Management.GetAll;
using AutoMapper;

namespace Application.Documents.Management.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<GetDocumentsPaginatedQuery, PaginateDTO>().ReverseMap();
        }
    }
}
