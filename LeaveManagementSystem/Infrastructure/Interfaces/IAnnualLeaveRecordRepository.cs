using LeaveManagementSystem.Domain;

namespace LeaveManagementSystem.Infrastructure.Interfaces
{
    public interface IAnnualLeaveRecordRepository
    {
        Task<AnnualLeaveRecord> CreateAsync(AnnualLeaveRecord annualLeaveRecord);
        Task<AnnualLeaveRecord> UpdateAsync(AnnualLeaveRecord annualLeaveRecord);
        Task<AnnualLeaveRecord> DeleteAsync(int id);
    }
}
