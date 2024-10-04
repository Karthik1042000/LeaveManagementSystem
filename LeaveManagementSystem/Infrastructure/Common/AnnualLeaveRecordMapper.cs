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
           .ForMember(dest => dest.Role, opt => opt.Ignore()); // Ignore Roles, since it is not in LeaveRecord

            // Mapping from LeaveRecord to LeaveRecordModel
            CreateMap<AnnualLeaveRecord, AnnualLeaveRecordModel>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name)) // Map Role object to string name
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
