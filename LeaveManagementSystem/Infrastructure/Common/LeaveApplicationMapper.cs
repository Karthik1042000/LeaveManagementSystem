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
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.Name)) // Map Employee Name
            .ForMember(dest => dest.Approver, opt => opt.MapFrom(src => src.Approver != null ? src.Approver.Name : null));

            // Mapping from LeaveApplicationModel (ViewModel) to LeaveApplication (Domain)
            CreateMap<LeaveApplicationModel, LeaveApplication>()
                .ForMember(dest => dest.Employee, opt => opt.Ignore()) // Employee handled separately
                .ForMember(dest => dest.Approver, opt => opt.Ignore());
        }
    }
}
