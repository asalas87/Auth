using Application.Security.Common.DTOS;
using Application.Security.Users.Create;
using AutoMapper;
using Domain.Secutiry.Entities;

namespace Application.Security.Users.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommand, RegisterDTO>().ReverseMap();
        CreateMap<GetUserByEmailQuery, LoginDTO>().ReverseMap();
    }
}