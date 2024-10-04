using LeaveManagementSystem.Infrastructure.DB;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LeaveManagementSystem.Infrastructure.Repositories.GenericRepositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    private IQueryable<T> _query;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _query = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _query.Where(predicate).ToListAsync();
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _query.Where(predicate).FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _query.AnyAsync(predicate);
    }
}
