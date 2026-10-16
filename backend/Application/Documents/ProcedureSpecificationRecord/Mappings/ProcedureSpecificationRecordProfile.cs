using Application.Common.Dtos;
using Application.Documents.ProcedureSpecificationRecord.Create;
using Application.Documents.ProcedureSpecificationRecord.Dtos;
using Application.Documents.ProcedureSpecificationRecord.GetAll;
using Application.Documents.ProcedureSpecificationRecord.Update;
using AutoMapper;

namespace Application.Documents.ProcedureSpecificationRecord.Mappings;

public class ProcedureSpecificationRecordProfile : Profile
{
    public ProcedureSpecificationRecordProfile()
    {
        CreateMap<GetProcedureSpecificationRecordsPaginatedQuery, PaginateDTO>().ReverseMap();
        CreateMap<ProcedureSpecificationRecordDTO, CreateProcedureSpecificationRecordCommand>();
        CreateMap<ProcedureSpecificationRecordEditDTO, UpdateProcedureSpecificationRecordCommand>();
    }
}