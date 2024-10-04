using LeaveManagementSystem.Domain;

namespace LeaveManagementSystem.Infrastructure.Interfaces
{
    public interface ILeaveApplicationRepository
    {
        Task<LeaveApplication> CreateAsync(LeaveApplication leaveApplication);
        Task<LeaveApplication> UpdateAsync(LeaveApplication leaveApplication);
        Task<LeaveApplication> DeleteAsync(int id);
        Task<LeaveApplication> ApproveAsync(int id, string userId);
        Task<LeaveApplication> RejectAsync(int id, string userId);
        Task<List<LeaveApplication>> GetLeaveApplicationsByEmployeeIdAsync(string id);
        Task<List<LeaveApplication>> GetPendingLeaveApplicationsAsync();
    }
}
