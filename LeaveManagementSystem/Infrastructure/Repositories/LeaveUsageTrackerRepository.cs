using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Infrastructure.Common;
using LeaveManagementSystem.Infrastructure.Interfaces;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;

namespace LeaveManagementSystem.Infrastructure.Repositories;

public class LeaveUsageTrackerRepository : ILeaveUsageTrackerRepository
{
    private readonly IWriteRepository<LeaveUsageTracker> writeRepository;
    private readonly IRepository<LeaveUsageTracker> repository;
    private readonly IRepository<Employee> employeeRepository;

    public LeaveUsageTrackerRepository(IWriteRepository<LeaveUsageTracker> writeRepository, 
        IRepository<LeaveUsageTracker> repository, 
        IRepository<Employee> employeeRepository)
    {
        this.writeRepository = writeRepository;
        this.repository = repository;
        this.employeeRepository = employeeRepository;
    }

    public async Task<LeaveUsageTracker> GetByIdAndYearAsync(string employeeId, int year)
    {
        var leaveTracker = await repository.FirstOrDefaultAsync(lt => lt.EmployeeId == employeeId && lt.Year == year);
        if (leaveTracker == null)
        {
            throw new Exception($"Leave Usage Tracker for EmployeeId {employeeId} in Year {year} not found.");
        }
        return leaveTracker;
    }

    public async Task<LeaveUsageTracker> CreateAsync(LeaveUsageTracker leaveUsageTracker)
    {
        var employee = await employeeRepository.FirstOrDefaultAsync(x=>x.Id == leaveUsageTracker.EmployeeId);
        if (employee == null)
        {
            throw new Exception($"The EmployeeId {leaveUsageTracker.EmployeeId} does not exist.");
        }

        var existingLeaveTracker = await repository.FirstOrDefaultAsync(
            x => x.EmployeeId == leaveUsageTracker.EmployeeId && x.Year == leaveUsageTracker.Year);
        if (existingLeaveTracker != null)
        {
            throw new Exception($"A Leave Usage Tracker entry for EmployeeId {leaveUsageTracker.EmployeeId} in Year {leaveUsageTracker.Year} already exists.");
        }

        return await writeRepository.CreateAsync(leaveUsageTracker);
    }

    public async Task<LeaveUsageTracker> UpdateAsync(LeaveUsageTracker leaveUsageTracker)
    {
        var existingLeaveTracker = await repository.GetByIdAsync(leaveUsageTracker.Id);
        if (existingLeaveTracker == null)
        {
            throw new Exception($"A Leave Usage Tracker Id {leaveUsageTracker.Id} was not found");
        }

        bool isUpdated = false;

        // Use the updated method to compare and update fields
        existingLeaveTracker.ALUsed = PropertyUpdater.UpdateIfChanged(existingLeaveTracker.ALUsed, leaveUsageTracker.ALUsed, ref isUpdated);
        existingLeaveTracker.RHUsed = PropertyUpdater.UpdateIfChanged(existingLeaveTracker.RHUsed, leaveUsageTracker.RHUsed, ref isUpdated);
        existingLeaveTracker.CLUsed = PropertyUpdater.UpdateIfChanged(existingLeaveTracker.CLUsed, leaveUsageTracker.CLUsed, ref isUpdated);
        existingLeaveTracker.BLUsed = PropertyUpdater.UpdateIfChanged(existingLeaveTracker.BLUsed, leaveUsageTracker.BLUsed, ref isUpdated);

        if (isUpdated)
        {
            return await writeRepository.UpdateAsync(existingLeaveTracker);
        }
        return existingLeaveTracker; // No changes made, return the existing tracker
    }

    public async Task<LeaveUsageTracker> DeleteAsync(int id)
    {
        var leaveTracker = await repository.FirstOrDefaultAsync(lt => lt.Id == id);
        if (leaveTracker == null)
        {
            throw new Exception($"Leave Usage Tracker Id {id} was not found.");
        }

        return await writeRepository.DeleteAsync(leaveTracker);
    }
}
