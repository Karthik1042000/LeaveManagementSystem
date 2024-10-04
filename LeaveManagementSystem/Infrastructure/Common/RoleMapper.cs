using AutoMapper;
using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Infrastructure.Common
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            CreateMap<RoleModel, Role>()
           .ForMember(dest => dest.State, opt => opt.Ignore());

            CreateMap<Role, RoleModel>();
        }
    }
}
