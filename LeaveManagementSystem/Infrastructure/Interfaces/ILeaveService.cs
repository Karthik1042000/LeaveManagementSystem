using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Infrastructure.Interfaces
{
    public interface ILeaveService
    {
        Task<List<LeaveUsageTrackerModel>> GetLeaveUsageTrackerListAsync();
        Task<LeaveUsageTrackerModel> GetLeaveUsageTrackerByIdAsync(int ltId);
        Task<List<LeaveUsageTrackerModel>> GetLeaveUsageTrackerByEmployeeIdAsync(string userId);
    }
}
