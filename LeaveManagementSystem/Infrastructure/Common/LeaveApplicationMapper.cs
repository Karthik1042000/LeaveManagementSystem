using AutoMapper;
using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Infrastructure.Common
{
    public class LeaveApplicationMapper : Profile
    {
        public LeaveApplicationMapper() 
        {
            CreateMap<LeaveApplication, LeaveApplicationModel>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.Name)) 
            .ForMember(dest => dest.Approver, opt => opt.MapFrom(src => src.Approver != null ? src.Approver.Name : null));
                       
            CreateMap<LeaveApplicationModel, LeaveApplication>()
                .ForMember(dest => dest.Employee, opt => opt.Ignore()) 
                .ForMember(dest => dest.Approver, opt => opt.Ignore());
        }
    }
}
