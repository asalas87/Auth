using Application.Common.Dtos;
using Application.Documents.ProcedureSpecification.Create;
using Application.Documents.ProcedureSpecification.Dtos;
using Application.Documents.ProcedureSpecification.GetAll;
using Application.Documents.ProcedureSpecification.Update;
using AutoMapper;

namespace Application.Documents.ProcedureSpecification.Mappings;

public class ProcedureSpecificationProfile : Profile
{
    public ProcedureSpecificationProfile()
    {
        CreateMap<GetProcedureSpecificationsPaginatedQuery, PaginateDTO>().ReverseMap();
        CreateMap<ProcedureSpecificationDTO, CreateProcedureSpecificationCommand>();
        CreateMap<ProcedureSpecificationEditDTO, UpdateProcedureSpecificationCommand>();
    }
}