using LeaveManagementSystem.Domain;

namespace LeaveManagementSystem.Models;

public class ProfileModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string RoleName { get; set; } = null!;
}

