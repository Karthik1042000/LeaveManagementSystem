using LeaveManagementSystem.Domain;
using System.Linq.Expressions;

namespace LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces
{
    public interface IRoleRepository
    {
        Task<Role> CreateAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task<Role> DeleteAsync(int id);
    }
}
