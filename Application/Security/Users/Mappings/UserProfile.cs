using Application.Common.Dtos;
using Application.Security.Common.DTOS;
using Application.Security.Users.Create;
using Application.Security.Users.GetAll;
using AutoMapper;

namespace Application.Security.Users.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommand, RegisterDTO>().ReverseMap();
        CreateMap<GetUserByEmailQuery, LoginDTO>().ReverseMap();
        CreateMap<GetUsersPaginatedQuery, PaginateDto>().ReverseMap();
    }
}