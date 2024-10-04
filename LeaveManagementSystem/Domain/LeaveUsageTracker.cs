
namespace LeaveManagementSystem.Domain
{
    public class LeaveUsageTracker
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        public int Year { get; set; }
        public int ALUsed { get; set; }
        public int RHUsed { get; set; }
        public int BLUsed { get; set; }
        public int CLUsed { get; set; }
    }
}
