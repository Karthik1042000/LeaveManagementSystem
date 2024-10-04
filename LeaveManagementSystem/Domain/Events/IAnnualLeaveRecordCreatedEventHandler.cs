namespace LeaveManagementSystem.Domain.Events
{
    public interface IAnnualLeaveRecordCreatedEventHandler
    {
        Task HandleAsync(AnnualLeaveRecordCreatedEvent AnnualLeaveRecordCreatedEvent);
    }
}
