namespace LeaveManagementSystem.Domain.Events
{
    public interface IEmployeeCreatedEventHandler
    {
        Task HandleAsync(EmployeeCreatedEvent employeeCreatedEvent);
    }
}
