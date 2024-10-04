using LeaveManagementSystem.Domain;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Models
{
    public class LeaveApplicationModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Types.LeaveTypes LeaveType { get; set; }
        public Types.State State { get; set; } = Types.State.Pending;

        // Who requested leave
        public string EmployeeId { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        // Who Approved leave
        public string? ApproverId { get; set; }
        public string? Approver { get; set; }
        public class Types
        {
            public enum LeaveTypes
            {
                [Display(Name = "Annual Leave")]
                AnnualLeave = 0,

                [Display(Name = "Casual Leave")]
                CasualLeave = 1,

                [Display(Name = "Restricted Holiday")]
                RestrictedHoliday = 2,

                [Display(Name = "Bonus Leave")]
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
