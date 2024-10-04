﻿namespace LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;

public interface IWriteRepository<T> where T : class
{
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
}
