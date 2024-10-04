using AutoMapper;
using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Infrastructure.Common
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {            
            CreateMap<Employee, ProfileModel>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name)); 

            CreateMap<ProfileModel, Employee>()
                .ForMember(dest => dest.Role, opt => opt.Ignore()); 
        }
    }
}
