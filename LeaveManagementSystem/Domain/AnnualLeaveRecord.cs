
namespace LeaveManagementSystem.Domain
{
    public class AnnualLeaveRecord
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public int Year { get; set; }
        public int AnnualLeave { get; set; }
        public int CasualLeave { get; set; }
        public int RestrictedHoliday { get; set; }
        public int BonusLeave { get; set; }

    }
}
