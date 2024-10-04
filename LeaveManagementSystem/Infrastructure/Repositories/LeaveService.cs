using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Infrastructure.DB;
using LeaveManagementSystem.Infrastructure.Interfaces;
using LeaveManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
namespace LeaveManagementSystem.Infrastructure.Repositories
{
    public class LeaveService : ILeaveService
    {
        private readonly ApplicationDbContext _context;

        public LeaveService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveUsageTrackerModel>> GetLeaveUsageTrackerListAsync()
        {
            var leaveUsageTrackers = await _context.LeaveUsageTracker
                .Select(lt => new LeaveUsageTrackerModel
                {
                    Id = lt.Id,
                    EmployeeId = lt.EmployeeId,
                    EmployeeName = lt.Employee.Name,
                    Year = lt.Year,
                    ALUsed = lt.ALUsed,
                    CLUsed = lt.CLUsed,
                    RHUsed = lt.RHUsed,
                    BLUsed = lt.BLUsed,
                    // Calculate the pending leaves by fetching the related LeaveRecord for the same year and role
                    ALPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.AnnualLeave - lt.ALUsed)
                        .FirstOrDefault(),
                    CLPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.CasualLeave - lt.CLUsed)
                        .FirstOrDefault(),
                    RHPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.RestrictedHoliday - lt.RHUsed)
                        .FirstOrDefault(),
                    BLPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.BonusLeave - lt.BLUsed)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return leaveUsageTrackers;
        }

        public async Task<LeaveUsageTrackerModel> GetLeaveUsageTrackerByIdAsync(int ltId)
        {
            var leaveUsageTrackers = await _context.LeaveUsageTracker
                .Where(lt => lt.Id == ltId)
                .Select(lt => new LeaveUsageTrackerModel
                {
                    Id = lt.Id,
                    EmployeeId = lt.EmployeeId,
                    EmployeeName = lt.Employee.Name, // Assuming Employee has a Name field
                    Year = lt.Year,
                    ALUsed = lt.ALUsed,
                    CLUsed = lt.CLUsed,
                    RHUsed = lt.RHUsed,
                    BLUsed = lt.BLUsed,
                    // Calculate the pending leaves by fetching the related LeaveRecord for the same year and role
                    ALPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.AnnualLeave - lt.ALUsed)
                        .FirstOrDefault(),
                    CLPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.CasualLeave - lt.CLUsed)
                        .FirstOrDefault(),
                    RHPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.RestrictedHoliday - lt.RHUsed)
                        .FirstOrDefault(),
                    BLPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.BonusLeave - lt.BLUsed)
                        .FirstOrDefault()
                })
            .FirstOrDefaultAsync();

            return leaveUsageTrackers!; // This will return null if no tracker is found
        }

        public async Task<List<LeaveUsageTrackerModel>> GetLeaveUsageTrackerByEmployeeIdAsync(string userId)
        {
            var leaveUsageTrackers = await _context.LeaveUsageTracker
                .Where(lt => lt.EmployeeId == userId)  // Fetch leave tracker records for the given user
                .Select(lt => new LeaveUsageTrackerModel
                {
                    Id = lt.Id,
                    EmployeeId = lt.EmployeeId,
                    EmployeeName = lt.Employee.Name, // Assuming Employee has a Name property
                    Year = lt.Year,
                    ALUsed = lt.ALUsed,
                    CLUsed = lt.CLUsed,
                    RHUsed = lt.RHUsed,
                    BLUsed = lt.BLUsed,
                    // Calculate pending leaves by fetching related LeaveRecord for the same year and role
                    ALPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.AnnualLeave - lt.ALUsed)
                        .FirstOrDefault(),
                    CLPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.CasualLeave - lt.CLUsed)
                        .FirstOrDefault(),
                    RHPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.RestrictedHoliday - lt.RHUsed)
                        .FirstOrDefault(),
                    BLPending = _context.AnnualLeaveRecord
                        .Where(lr => lr.RoleId == lt.Employee.RoleId && lr.Year == lt.Year)
                        .Select(lr => lr.BonusLeave - lt.BLUsed)
                        .FirstOrDefault()
                })
                .ToListAsync();  // Fetch all records as a list

            return leaveUsageTrackers;
        }

    }
}
