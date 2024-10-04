using LeaveManagementSystem.Infrastructure.DB;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.Repositories.GenericRepositories;

public class WriteRepository<T> : IWriteRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    private IQueryable<T> _query;

    public WriteRepository(ApplicationDbContext context)
    {
        _context = context;
        _query = _context.Set<T>();
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();
        return entities;
    }

}
