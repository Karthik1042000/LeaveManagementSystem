using AutoMapper;
using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Infrastructure.Common
{
    public class LeaveUsageTrackerMapper : Profile
    {
        public LeaveUsageTrackerMapper()
        {
            CreateMap<LeaveUsageTracker, LeaveUsageTrackerModel>()
           .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.Name))
           .ForMember(dest => dest.ALPending, opt => opt.Ignore()) 
           .ForMember(dest => dest.CLPending, opt => opt.Ignore())
           .ForMember(dest => dest.RHPending, opt => opt.Ignore())
           .ForMember(dest => dest.BLPending, opt => opt.Ignore());

            CreateMap<LeaveUsageTrackerModel, LeaveUsageTracker>()
                .ForMember(dest => dest.Employee, opt => opt.Ignore()); 
        }
    }
}
