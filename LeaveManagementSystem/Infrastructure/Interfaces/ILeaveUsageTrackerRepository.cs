using LeaveManagementSystem.Domain;

namespace LeaveManagementSystem.Infrastructure.Interfaces
{
    public interface ILeaveUsageTrackerRepository
    {
        Task<LeaveUsageTracker> CreateAsync(LeaveUsageTracker leaveUsageTracker);
        Task<LeaveUsageTracker> GetByIdAndYearAsync(string employeeId, int year);
        Task<LeaveUsageTracker> UpdateAsync(LeaveUsageTracker leaveUsageTracker);
        Task<LeaveUsageTracker> DeleteAsync(int id);
    }
}
