using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Domain.Events;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;

namespace LeaveManagementSystem.Infrastructure.EventHandlers
{
    public class AnnualLeaveRecordCreatedEventHandler : IAnnualLeaveRecordCreatedEventHandler
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IWriteRepository<LeaveUsageTracker> _leaveUsageTrackerRepository;
        private readonly ILogger<AnnualLeaveRecordCreatedEventHandler> _logger;

        public AnnualLeaveRecordCreatedEventHandler(
            IRepository<Employee> employeeRepository,
            IWriteRepository<LeaveUsageTracker> leaveTrackerRepository,
            ILogger<AnnualLeaveRecordCreatedEventHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _leaveUsageTrackerRepository = leaveTrackerRepository;
            _logger = logger;
        }

        public async Task HandleAsync(AnnualLeaveRecordCreatedEvent leaveRecordCreatedEvent)
        {
            if (leaveRecordCreatedEvent == null)
            {
                _logger.LogWarning("Received null LeaveRecordCreatedEvent.");
                return;
            }

            var annualLeaveRecord = leaveRecordCreatedEvent.AnnualLeaveRecord;
            if (annualLeaveRecord == null)
            {
                _logger.LogWarning("AnnualLeaveRecord object in LeaveRecordCreatedEvent is null.");
                return;
            }

            try
            {
                // Fetch all employees with the specified RoleId
                var employeeList = await _employeeRepository.FindAsync(x => x.Role.Id == annualLeaveRecord.RoleId);
                var leaveUsageTrackers = new List<LeaveUsageTracker>(); 

                foreach (var employee in employeeList)
                {
                    var leaveTracker = new LeaveUsageTracker
                    {
                        EmployeeId = employee.Id,
                        ALUsed = 0,
                        BLUsed = 0,
                        CLUsed = 0,
                        RHUsed = 0,
                        Year = annualLeaveRecord.Year
                    };
                    leaveUsageTrackers.Add(leaveTracker); 
                }

                // Add the leave trackers to the repository
                await _leaveUsageTrackerRepository.AddRangeAsync(leaveUsageTrackers); 
                _logger.LogInformation($"LeaveUsageTrackers created for LeaveRecordId {annualLeaveRecord.Id} for year {annualLeaveRecord.Year}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling LeaveRecordCreatedEvent for LeaveRecord {LeaveRecordId}.", annualLeaveRecord.Id);
            }
        }

    }
}
