
namespace LeaveManagementSystem.Domain
{
    public class Employee
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public Types.State State { get; set; } = Types.State.Active;
        public class Types
        {
            public enum State
            {
                Active = 0,
                InActive = 1,
            }
        }
    }
    
}
