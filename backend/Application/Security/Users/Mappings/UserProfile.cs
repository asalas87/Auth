using Application.Common.Dtos;
using Application.Security.Common.DTOs;
using Application.Security.Common.DTOS;
using Application.Security.Users.Create;
using Application.Security.Users.Edit;
using Application.Security.Users.GetAll;
using AutoMapper;
using Domain.Security.Entities;

namespace Application.Security.Users.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
                                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                                  .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name))
                                  .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : string.Empty));
        CreateMap<User, UserEditDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
                                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                                  .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Role.Id))
                                  .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Company.Id.Value));
        CreateMap<RegisterDTO, CreateUserCommand>();
        CreateMap<GetUserByEmailQuery, LoginDTO>().ReverseMap();
        CreateMap<GetUsersPaginatedQuery, PaginateDTO>().ReverseMap();
        CreateMap<EditUserCommand, EditUserRequest>().ReverseMap();
    }
}
