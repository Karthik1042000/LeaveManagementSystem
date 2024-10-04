using LeaveManagementSystem.Domain;

namespace LeaveManagementSystem.Models
{
    public class EmployeeModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public string Role { get; set; } = null!;
        public List<Role>? Roles { get; set; }
    }
}
