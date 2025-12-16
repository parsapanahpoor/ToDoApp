using AutoMapper;
using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Model.Account;

namespace ToDoApp.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // UserEntity to EditUserDto
        CreateMap<UserEntity, EditUserDto>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.SelectedRoles, opt => opt.Ignore());

        // CreateUserDto to UserEntity
        CreateMap<CreateUserDto, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDelete, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
            .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => "default-avatar.png"));

        // RegisterUserDto to UserEntity
        CreateMap<RegisterUserDto, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDelete, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
            .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => "default-avatar.png"));
    }
}
