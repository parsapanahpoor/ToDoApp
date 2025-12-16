using AutoMapper;
using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Model.Account;

namespace ToDoApp.Application.Mappings;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        // Role to EditRoleDto
        CreateMap<Role, EditRoleDto>();

        // CreateRoleDto to Role
        CreateMap<CreateRoleDto, Role>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDelete, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore());
    }
}
