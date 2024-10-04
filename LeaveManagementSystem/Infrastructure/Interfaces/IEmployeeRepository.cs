using LeaveManagementSystem.Domain;

namespace LeaveManagementSystem.Infrastructure.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(string id);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);
        Task<Employee> DeleteAsync(string id);
        Task<Employee> SignInAsync(string id, string password);
    }
}
