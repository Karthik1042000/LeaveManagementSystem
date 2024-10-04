using AutoMapper;
using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Infrastructure.Common
{
    public class EmployeeMapper:Profile
    {
        public EmployeeMapper()
        {
            CreateMap<EmployeeModel, Employee>()
           .ForMember(dest => dest.Role, opt => opt.Ignore())
           .ForMember(dest => dest.State, opt => opt.Ignore());

            CreateMap<Employee, EmployeeModel>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name)) // Map Role object to string name
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
