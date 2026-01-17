using AutoMapper;
using Back.DTOs.Users;
using Back.Models;

namespace Back.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Crear User desde CreateUserDto
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            // User → UserResponseDTO (incluye roles)
            CreateMap<User, UserReponseDTO>()
                .ForMember(dest => dest.Roles,
                    opt => opt.MapFrom(src =>
                        src.UserRoles != null
                            ? src.UserRoles
                                .Where(ur =>
                                    ur.IsActive &&
                                    !ur.IsDeleted &&
                                    ur.Role != null &&
                                    ur.Role.IsActive &&
                                    !ur.Role.IsDeleted
                                )
                                .Select(ur => ur.Role!.Name)
                                .ToList()
                            : new List<string>()
                    )
                );

            // User → UserListDto (listas simples)
            CreateMap<User, UserListDto>();
        }
    }
}
