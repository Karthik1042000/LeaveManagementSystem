using AutoMapper;
using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Infrastructure.Common
{
    public class AnnualLeaveRecordMapper : Profile
    {
        public AnnualLeaveRecordMapper()
        {
            CreateMap<AnnualLeaveRecordModel, AnnualLeaveRecord>()
           .ForMember(dest => dest.Role, opt => opt.Ignore()); 

            CreateMap<AnnualLeaveRecord, AnnualLeaveRecordModel>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name)) 
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
