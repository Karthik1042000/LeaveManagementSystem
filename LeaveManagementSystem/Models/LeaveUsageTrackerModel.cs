namespace LeaveManagementSystem.Models
{
    public class LeaveUsageTrackerModel
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public int Year { get; set; }
        public int ALUsed { get; set; }
        public int CLUsed { get; set; }
        public int RHUsed { get; set; }
        public int BLUsed { get; set; }
        public int ALPending { get; set; }
        public int CLPending { get; set; }
        public int RHPending { get; set; }
        public int BLPending { get; set; }
    }
}
