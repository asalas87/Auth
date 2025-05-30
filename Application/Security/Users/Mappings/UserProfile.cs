using Application.Common.Dtos;
using Application.Security.Common.DTOS;
using Application.Security.Users.Create;
using Application.Security.Users.GetAll;
using AutoMapper;
using Domain.Security.Entities;

namespace Application.Security.Users.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
                                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
        CreateMap<CreateUserCommand, RegisterDTO>().ReverseMap();
        CreateMap<GetUserByEmailQuery, LoginDTO>().ReverseMap();
        CreateMap<GetUsersPaginatedQuery, PaginateDTO>().ReverseMap();
    }
}