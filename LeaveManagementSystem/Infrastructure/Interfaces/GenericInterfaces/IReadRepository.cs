using System.Linq.Expressions;

namespace LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces
{
    public interface IReadRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> ListAsync();
    }
}
