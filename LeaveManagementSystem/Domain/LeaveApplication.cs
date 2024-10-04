
namespace LeaveManagementSystem.Domain
{
    public class LeaveApplication
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Types.LeaveTypes LeaveType { get; set; }
        public Types.State State { get; set; } = Types.State.Pending;

        // Who requested leave
        public string EmployeeId { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        // Who Approved leave
        public string? ApproverId { get; set; }
        public Employee? Approver { get; set; }

        public class Types
        {
            public enum LeaveTypes
            {
                AnnualLeave = 0,
                CasualLeave = 1,
                RestrictedHoliday = 2,
                BonusLeave = 3
            }
            public enum State
            {
                Pending = 0,
                Approved = 1,
                Rejected = 2
            }
        }
        
    }
    
}
