using LeaveManagementSystem.Domain;

namespace LeaveManagementSystem.Models
{
    public class AnnualLeaveRecordModel
    {
        public int Id { get; set; }
        public string Role { get; set; } = null!;
        public List<Role>? Roles { get; set; }
        public int RoleId { get; set; }
        public int Year { get; set; }
        public int AnnualLeave { get; set; }
        public int CasualLeave { get; set; }
        public int RestrictedHoliday { get; set; }
        public int BonusLeave { get; set; }
    }
}
