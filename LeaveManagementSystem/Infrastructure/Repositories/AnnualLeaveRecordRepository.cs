using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Domain.Events;
using LeaveManagementSystem.Infrastructure.Common;
using LeaveManagementSystem.Infrastructure.Interfaces;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;

namespace LeaveManagementSystem.Infrastructure.Repositories
{
    public class AnnualLeaveRecordRepository : IAnnualLeaveRecordRepository
    {
        private readonly IWriteRepository<AnnualLeaveRecord> writeRepository;
        private readonly IRepository<AnnualLeaveRecord> repository;
        private readonly IReadRepository<Role> roleRepository;
        private readonly IAnnualLeaveRecordCreatedEventHandler annualLeaveRecordCreatedEventHandler;

        public AnnualLeaveRecordRepository(IWriteRepository<AnnualLeaveRecord> writeRepository, IRepository<AnnualLeaveRecord> repository,
            IReadRepository<Role> roleRepository,IAnnualLeaveRecordCreatedEventHandler leaveRecordCreatedEventHandler)
        {
            this.writeRepository = writeRepository;
            this.repository = repository;
            this.roleRepository = roleRepository;
            this.annualLeaveRecordCreatedEventHandler = leaveRecordCreatedEventHandler;
        }

        public async Task<AnnualLeaveRecord> CreateAsync(AnnualLeaveRecord annualLeaveRecord)
        {
            if (annualLeaveRecord.Year < DateTime.UtcNow.Year || annualLeaveRecord.Year > 9999)
            {
                throw new Exception($"Year must be between {DateTime.UtcNow.Year} and 9999.");
            }

            // Check if the RoleId exists
            var role = await roleRepository.GetByIdAsync(annualLeaveRecord.RoleId);
            if (role == null)
            {
                throw new Exception($"The RoleId {annualLeaveRecord.RoleId} does not exist.");
            }

            // Check if the combination of Year and RoleId already exists
            bool exists = await repository.ExistsAsync(lr => lr.Year == annualLeaveRecord.Year && lr.RoleId == annualLeaveRecord.RoleId);
            if (exists)
            {
                throw new Exception($"Annual Leave record for RoleId {annualLeaveRecord.RoleId} and Year {annualLeaveRecord.Year} already exists.");
            }

            annualLeaveRecord = await writeRepository.CreateAsync(annualLeaveRecord);
            var annualLeaveRecordCreatedEvent = new AnnualLeaveRecordCreatedEvent(annualLeaveRecord);
            await annualLeaveRecordCreatedEventHandler.HandleAsync(annualLeaveRecordCreatedEvent);
            return annualLeaveRecord;
        }

        public async Task<AnnualLeaveRecord> UpdateAsync(AnnualLeaveRecord annualLeaveRecord)
        {
            var existingLeaveRecord = await repository.FirstOrDefaultAsync(lr => lr.Id == annualLeaveRecord.Id);
            if (existingLeaveRecord == null)
            {
                throw new Exception($"Annual Leave record with Id: {annualLeaveRecord.Id} not found.");
            }

            // Check if Year is within the valid range
            if (annualLeaveRecord.Year < DateTime.UtcNow.Year || annualLeaveRecord.Year > 9999)
            {
                throw new Exception($"Year must be between {DateTime.UtcNow.Year} and 9999.");
            }

            bool isUpdated = false;

            // Use the updated method to compare and update fields
            existingLeaveRecord.AnnualLeave = PropertyUpdater.UpdateIfChanged(existingLeaveRecord.AnnualLeave, annualLeaveRecord.AnnualLeave, ref isUpdated);
            existingLeaveRecord.CasualLeave = PropertyUpdater.UpdateIfChanged(existingLeaveRecord.CasualLeave, annualLeaveRecord.CasualLeave, ref isUpdated);
            existingLeaveRecord.RestrictedHoliday = PropertyUpdater.UpdateIfChanged(existingLeaveRecord.RestrictedHoliday, annualLeaveRecord.RestrictedHoliday, ref isUpdated);
            existingLeaveRecord.BonusLeave = PropertyUpdater.UpdateIfChanged(existingLeaveRecord.BonusLeave, annualLeaveRecord.BonusLeave, ref isUpdated);
            existingLeaveRecord.Year = PropertyUpdater.UpdateIfChanged(existingLeaveRecord.Year, annualLeaveRecord.Year, ref isUpdated);
            existingLeaveRecord.RoleId = PropertyUpdater.UpdateIfChanged(existingLeaveRecord.RoleId, annualLeaveRecord.RoleId, ref isUpdated);

            // Check if the RoleId exists
            var role = await roleRepository.GetByIdAsync(existingLeaveRecord.RoleId);
            if (role == null)
            {
                throw new Exception($"The RoleId {existingLeaveRecord.RoleId} does not exist.");
            }

            // Check if the Year and RoleId combination already exists (excluding the current record)
            bool exists = await repository.ExistsAsync(lr => lr.Year == existingLeaveRecord.Year && lr.RoleId == existingLeaveRecord.RoleId && lr.Id != existingLeaveRecord.Id);
            if (exists)
            {
                throw new Exception($"Annual Leave record for RoleId {existingLeaveRecord.RoleId} and Year {existingLeaveRecord.Year} already exists.");
            }
            if (isUpdated)
            {
                return await writeRepository.UpdateAsync(existingLeaveRecord);
            }

            return existingLeaveRecord;
        }

        public async Task<AnnualLeaveRecord> DeleteAsync(int id)
        {
            var leaveRecord = await repository.GetByIdAsync(id);
            if (leaveRecord == null)
            {
                throw new Exception($"Annual Leave record with Id: {id} not found.");
            }

            return await writeRepository.DeleteAsync(leaveRecord);
        }

    }
    
}
