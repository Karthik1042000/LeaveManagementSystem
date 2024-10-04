using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Domain.Events;
using LeaveManagementSystem.Infrastructure.Interfaces;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;

namespace LeaveManagementSystem.Infrastructure.EventHandlers
{
    public class EmployeeCreatedEventHandler : IEmployeeCreatedEventHandler
    {
        private readonly ILeaveUsageTrackerRepository leaveUsageTrackerRepository;
        private readonly IRepository<AnnualLeaveRecord> annualLeaveRecordRepository;
        private readonly ILogger<EmployeeCreatedEventHandler> logger;

        public EmployeeCreatedEventHandler(ILeaveUsageTrackerRepository leaveTrackerRepository,
                                       IRepository<AnnualLeaveRecord> annualLeaveRecordRepository,
                                       ILogger<EmployeeCreatedEventHandler> logger)
        {
            this.leaveUsageTrackerRepository = leaveTrackerRepository;
            this.annualLeaveRecordRepository = annualLeaveRecordRepository;
            this.logger = logger;
        }

        public async Task HandleAsync(EmployeeCreatedEvent employeeCreatedEvent)
        {
            if (employeeCreatedEvent == null)
            {
                logger.LogWarning("Received null EmployeeCreatedEvent.");
                return;
            }

            var employee = employeeCreatedEvent.NewEmployee;
            if (employee == null)
            {
                logger.LogWarning("NewEmployee object in EmployeeCreatedEvent is null.");
                return;
            }

            try
            {
                var annuaLeaveRecord = await annualLeaveRecordRepository.FirstOrDefaultAsync(x => x.RoleId == employee.RoleId && x.Year == DateTime.UtcNow.Year);
                if (annuaLeaveRecord == null)
                {
                    logger.LogInformation($"No AnnualLeaveRecord found for RoleId {employee.RoleId} for the year {DateTime.UtcNow.Year}. No LeaveUsageTracker created.");
                    return;
                }

                var leaveUsageTracker = new LeaveUsageTracker()
                {
                    EmployeeId = employee.Id,
                    ALUsed = 0,
                    BLUsed = 0,
                    CLUsed = 0,
                    RHUsed = 0,
                    Year = DateTime.UtcNow.Year
                };

                await leaveUsageTrackerRepository.CreateAsync(leaveUsageTracker);
                logger.LogInformation($"LeaveUsageTracker created for EmployeeId {employee.Id} for year {leaveUsageTracker.Year}.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while handling EmployeeCreatedEvent for EmployeeId {EmployeeId}.", employee.Id);
                throw; 
            }

        }
    }
}
