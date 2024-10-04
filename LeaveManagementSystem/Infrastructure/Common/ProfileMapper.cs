using AutoMapper;
using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Infrastructure.Common
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            // Map from User to UserProfile
            CreateMap<Employee, ProfileModel>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name)); // Map Role.Name to RoleName

            // Optional: You can map UserProfile back to User if needed (ignore Role as it is not part of UserProfile)
            CreateMap<ProfileModel, Employee>()
                .ForMember(dest => dest.Role, opt => opt.Ignore()); 
        }
    }
}
