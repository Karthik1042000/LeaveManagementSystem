using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Infrastructure.Common;
using LeaveManagementSystem.Infrastructure.Interfaces;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;
using LeaveManagementSystem.Infrastructure.Repositories.GenericRepositories;

namespace LeaveManagementSystem.Infrastructure.Repositories
{
    public class LeaveApplicationRepository : ILeaveApplicationRepository
    {
        private readonly IWriteRepository<LeaveApplication> leaveApplicationWriteRepository;
        private readonly IRepository<LeaveApplication> leaveApplicationRepository;
        private readonly IReadRepository<Employee> employeeRepository;
        private readonly IReadRepository<LeaveUsageTracker> leaveUsageTrackerRepository;
        private readonly IReadRepository<AnnualLeaveRecord> annualLeaveRecordRepository;
        private readonly IWriteRepository<LeaveUsageTracker> leaveUsageTrackerWriteRepository;

        public LeaveApplicationRepository(IWriteRepository<LeaveApplication> leaveApplicationWriteRepository,
            IRepository<LeaveApplication> leaveApplicationRepository, IReadRepository<Employee> employeeRepository,
            IReadRepository<LeaveUsageTracker> leaveUsageTrackerRepository, IReadRepository<AnnualLeaveRecord> annualLeaveRecordRepository,
            IWriteRepository<LeaveUsageTracker> leaveUsageTrackerWriteRepository)
        {
            this.leaveApplicationWriteRepository = leaveApplicationWriteRepository;
            this.leaveApplicationRepository = leaveApplicationRepository;
            this.employeeRepository = employeeRepository;
            this.leaveUsageTrackerRepository = leaveUsageTrackerRepository;
            this.annualLeaveRecordRepository = annualLeaveRecordRepository;
            this.leaveUsageTrackerWriteRepository = leaveUsageTrackerWriteRepository;
        }
        public async Task<LeaveApplication> CreateAsync(LeaveApplication leaveApplication)
        {
            var employee = await employeeRepository.FirstOrDefaultAsync(x => x.Id == leaveApplication.EmployeeId);
            if (employee == null)
            {
                throw new ArgumentException($"Employee with ID {leaveApplication.Id} does not exist.");
            }

            if (leaveApplication.StartDate < DateTime.Today)
            {
                throw new ArgumentException("Start date must be a future date.");
            }

            if (leaveApplication.EndDate < leaveApplication.StartDate)
            {
                throw new ArgumentException("End date must be on or after the start date.");
            }

            leaveApplication.State = LeaveApplication.Types.State.Pending;

            int dayCount = (leaveApplication.EndDate - leaveApplication.StartDate).Days + 1;

            var leaveTracker = await leaveUsageTrackerRepository.FirstOrDefaultAsync(x =>
                x.EmployeeId == leaveApplication.EmployeeId &&
                x.Year == leaveApplication.StartDate.Year);

            var leaveRecord = await annualLeaveRecordRepository.FirstOrDefaultAsync(x =>
                x.RoleId == employee.RoleId &&
                x.Year == leaveApplication.StartDate.Year);

            var pendingApplications = await leaveApplicationRepository.FindAsync(x =>
                x.EmployeeId == leaveApplication.EmployeeId &&
                x.State == LeaveApplication.Types.State.Pending &&
                x.LeaveType == leaveApplication.LeaveType &&
                x.StartDate.Year == leaveApplication.StartDate.Year);

            if (leaveTracker != null && leaveRecord != null)
            {
                // Define a dictionary for leave types and their corresponding used/total leaves
                var leaveDetails = new Dictionary<LeaveApplication.Types.LeaveTypes, (int Used, int Total)>
                {
                    { LeaveApplication.Types.LeaveTypes.AnnualLeave, (leaveTracker.ALUsed, leaveRecord.AnnualLeave) },
                    { LeaveApplication.Types.LeaveTypes.CasualLeave, (leaveTracker.CLUsed, leaveRecord.CasualLeave) },
                    { LeaveApplication.Types.LeaveTypes.BonusLeave, (leaveTracker.BLUsed, leaveRecord.BonusLeave) },
                    { LeaveApplication.Types.LeaveTypes.RestrictedHoliday, (leaveTracker.RHUsed, leaveRecord.RestrictedHoliday) }
                };

                // Validate leave count
                if (leaveDetails.TryGetValue(leaveApplication.LeaveType, out var leaveData))
                {
                    // Initialize total leave used with the already used leaves.
                    int totalLeaveUsedIncludingPending = leaveData.Used;

                    // Check if there are any pending applications and sum their leave days.
                    if (pendingApplications.Any())
                    {
                        var pendingLeaveDays = pendingApplications.Sum(x => (x.EndDate - x.StartDate).Days + 1);
                        totalLeaveUsedIncludingPending += pendingLeaveDays;
                    }

                    // Check if the total leaves (used + pending) exceed the available leaves.
                    if (dayCount > (leaveData.Total - totalLeaveUsedIncludingPending))
                    {
                        throw new InvalidOperationException($"You have {leaveData.Total - totalLeaveUsedIncludingPending} {leaveApplication.LeaveType} days left, including pending applications.");
                    }
                }
            }

            return await leaveApplicationWriteRepository.CreateAsync(leaveApplication);
        }


        public async Task<LeaveApplication> UpdateAsync(LeaveApplication leaveApplication)
        {
            var existingLeaveApplication = await leaveApplicationRepository.FirstOrDefaultAsync(x => x.Id == leaveApplication.Id);
            if (existingLeaveApplication == null)
            {
                throw new KeyNotFoundException($"Leave application with ID: {leaveApplication.Id} not found.");
            }

            if (existingLeaveApplication.State != LeaveApplication.Types.State.Pending)
            {
                throw new InvalidOperationException($"Leave application with ID: {leaveApplication.Id} is already {existingLeaveApplication.State}.");
            }

            bool isUpdated = false;

            // Use the updated method to compare and update fields
            existingLeaveApplication.StartDate = PropertyUpdater.UpdateIfChanged(existingLeaveApplication.StartDate, leaveApplication.StartDate, ref isUpdated);
            existingLeaveApplication.EndDate = PropertyUpdater.UpdateIfChanged(existingLeaveApplication.EndDate, leaveApplication.EndDate, ref isUpdated);
            existingLeaveApplication.LeaveType = PropertyUpdater.UpdateIfChanged(existingLeaveApplication.LeaveType, leaveApplication.LeaveType, ref isUpdated);

            if (existingLeaveApplication.StartDate < DateTime.Today)
            {
                throw new ArgumentException("Start date must be a future date.");
            }

            if (existingLeaveApplication.EndDate < existingLeaveApplication.StartDate)
            {
                throw new ArgumentException("End date must be on or after the start date.");
            }

            int dayCount = (existingLeaveApplication.EndDate - existingLeaveApplication.StartDate).Days + 1;

            var leaveTracker = await leaveUsageTrackerRepository.FirstOrDefaultAsync(x =>
                x.EmployeeId == existingLeaveApplication.EmployeeId &&
                x.Year == existingLeaveApplication.StartDate.Year);

            var leaveRecord = await annualLeaveRecordRepository.FirstOrDefaultAsync(x =>
                x.RoleId == existingLeaveApplication.Employee.RoleId &&
                x.Year == existingLeaveApplication.StartDate.Year);

            if (leaveTracker != null && leaveRecord != null)
            {
                // Define a dictionary for leave types and their corresponding used/total leaves
                var leaveDetails = new Dictionary<LeaveApplication.Types.LeaveTypes, (int Used, int Total)>
                {
                    { LeaveApplication.Types.LeaveTypes.AnnualLeave, (leaveTracker.ALUsed, leaveRecord.AnnualLeave) },
                    { LeaveApplication.Types.LeaveTypes.CasualLeave, (leaveTracker.CLUsed, leaveRecord.CasualLeave) },
                    { LeaveApplication.Types.LeaveTypes.BonusLeave, (leaveTracker.BLUsed, leaveRecord.BonusLeave) },
                    { LeaveApplication.Types.LeaveTypes.RestrictedHoliday, (leaveTracker.RHUsed, leaveRecord.RestrictedHoliday) }
                };

                // Validate leave count
                if (leaveDetails.TryGetValue(existingLeaveApplication.LeaveType, out var leaveData))
                {
                    if (dayCount > (leaveData.Total - leaveData.Used))
                    {
                        throw new InvalidOperationException($"You only have {leaveData.Total - leaveData.Used} {existingLeaveApplication.LeaveType} days left.");
                    }
                }
            }

            if (isUpdated)
            {
                return await leaveApplicationWriteRepository.UpdateAsync(existingLeaveApplication);
            }

            return existingLeaveApplication;
        }


        public async Task<LeaveApplication> DeleteAsync(int id)
        {
            var leaveApplication = await leaveApplicationRepository.GetByIdAsync(id);
            if (leaveApplication == null)
            {
                throw new KeyNotFoundException($"Leave application with ID: {id} not found.");
            }

            return await leaveApplicationWriteRepository.DeleteAsync(leaveApplication);
        }

        public async Task<List<LeaveApplication>> GetLeaveApplicationsByEmployeeIdAsync(string employeeId)
        {
            var leaveApplications = await leaveApplicationRepository.FindAsync(x => x.EmployeeId == employeeId);
            return leaveApplications.ToList();
        }

        public async Task<List<LeaveApplication>> GetPendingLeaveApplicationsAsync()
        {
            var leaveApplications = await leaveApplicationRepository.FindAsync(x => x.State == LeaveApplication.Types.State.Pending);
            return leaveApplications.ToList();
        }


        public async Task<LeaveApplication> ApproveAsync(int id, string employeeId)
        {
            var employee = await employeeRepository.FirstOrDefaultAsync(x => x.Id == employeeId);
            if (employee == null)
            {
                throw new Exception($"The EmployeeId {employeeId} does not exist.");
            }
            // Fetch the leave application by ID
            var leaveApplication = await leaveApplicationRepository.GetByIdAsync(id);
            if (leaveApplication == null)
            {
                throw new Exception($"LeaveApplication with Id: {id} not found.");
            }

            // Check if the leave application is already approved
            if (leaveApplication.State == LeaveApplication.Types.State.Approved)
            {
                throw new Exception($"LeaveApplication with Id: {id} is already approved.");
            }

            // Approve the leave application
            var leaveTracker = await leaveUsageTrackerRepository.FirstOrDefaultAsync(x => x.EmployeeId == leaveApplication.EmployeeId &&
            x.Year == leaveApplication.StartDate.Year);
            int dayCount = (leaveApplication.EndDate - leaveApplication.StartDate).Days + 1;
            // Update the leave type usage based on the leave type
            switch (leaveApplication.LeaveType)
            {
                case LeaveApplication.Types.LeaveTypes.AnnualLeave:
                    leaveTracker!.ALUsed += dayCount;
                    break;
                case LeaveApplication.Types.LeaveTypes.BonusLeave:
                    leaveTracker!.BLUsed += dayCount;
                    break;
                case LeaveApplication.Types.LeaveTypes.RestrictedHoliday:
                    leaveTracker!.RHUsed += dayCount;
                    break;
                case LeaveApplication.Types.LeaveTypes.CasualLeave:
                    leaveTracker!.CLUsed += dayCount;
                    break;
                default:
                    throw new Exception("Invalid leave type.");
            }
            leaveApplication.ApproverId = employeeId;
            leaveApplication.State = LeaveApplication.Types.State.Approved;

            await leaveUsageTrackerWriteRepository.UpdateAsync(leaveTracker!);
            // Update the leave application in the repository
            return await leaveApplicationWriteRepository.UpdateAsync(leaveApplication);
        }

        public async Task<LeaveApplication> RejectAsync(int id, string employeeId)
        {
            var employee = await employeeRepository.FirstOrDefaultAsync(x => x.Id == employeeId);
            if (employee == null)
            {
                throw new Exception($"The EmployeeId {employeeId} does not exist.");
            }
            // Fetch the leave application by ID
            var leaveApplication = await leaveApplicationRepository.GetByIdAsync(id);
            if (leaveApplication == null)
            {
                throw new Exception($"LeaveApplication with Id: {id} not found.");
            }

            // Check if the leave application is already rejected
            if (leaveApplication.State == LeaveApplication.Types.State.Rejected)
            {
                throw new Exception($"LeaveApplication with Id: {id} is already rejected.");
            }

            leaveApplication.ApproverId = employeeId;
            // Reject the leave application
            leaveApplication.State = LeaveApplication.Types.State.Rejected;

            // Update the leave application in the repository
            return await leaveApplicationWriteRepository.UpdateAsync(leaveApplication);
        }
    }
}
