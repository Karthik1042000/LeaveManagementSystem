using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Domain.Events;
using LeaveManagementSystem.Infrastructure.Common;
using LeaveManagementSystem.Infrastructure.EventHandlers;
using LeaveManagementSystem.Infrastructure.Exceptions;
using LeaveManagementSystem.Infrastructure.Interfaces;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;

namespace LeaveManagementSystem.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IWriteRepository<Employee> employeeWriteRepository;
    private readonly IRepository<Employee> employeeRepository;
    private readonly IReadRepository<Role> roleRepository;
    private readonly IEmployeeCreatedEventHandler employeeCreatedEventHandler;

    public EmployeeRepository(IWriteRepository<Employee> employeeWriteRepository,
                              IRepository<Employee> employeeRepository,
                              IReadRepository<Role> roleRepository,
                              IEmployeeCreatedEventHandler employeeCreatedEventHandler)
    {
        this.employeeWriteRepository = employeeWriteRepository;
        this.employeeRepository = employeeRepository;
        this.roleRepository = roleRepository;
        this.employeeCreatedEventHandler = employeeCreatedEventHandler;
    }

    public async Task<Employee> GetByIdAsync(string id)
    {
        var employee = await employeeRepository.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            throw new Exception($"The Employee with ID {id} does not exist.");
        }
        return employee;
    }

    public async Task<Employee> CreateAsync(Employee employee)
    {
        if (await employeeRepository.ExistsAsync(e => e.Email == employee.Email))
        {
            throw new Exception($"The email {employee.Email} is already registered.");
        }

        var role = await roleRepository.GetByIdAsync(employee.RoleId);
        if (role == null)
        {
            throw new Exception($"The Role ID {employee.RoleId} is invalid.");
        }

        employee.Id = GenerateRandomId();
        employee.State = Employee.Types.State.Active;

        employee = await employeeWriteRepository.CreateAsync(employee);

        var employeeCreatedEvent = new EmployeeCreatedEvent(employee);
        await employeeCreatedEventHandler.HandleAsync(employeeCreatedEvent);
        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        var existingEmployee = await employeeRepository.FirstOrDefaultAsync(e => e.Id == employee.Id);
        if (existingEmployee == null)
        {
            throw new Exception($"Employee with ID {employee.Id} not found.");
        }

        bool isUpdated = false;

        existingEmployee.Name = PropertyUpdater.UpdateIfChanged(existingEmployee.Name, employee.Name, ref isUpdated);
        existingEmployee.Password = PropertyUpdater.UpdateIfChanged(existingEmployee.Password, employee.Password, ref isUpdated);
        existingEmployee.State = PropertyUpdater.UpdateIfChanged(existingEmployee.State, employee.State, ref isUpdated);

        if (employee.RoleId != existingEmployee.RoleId && employee.RoleId != 0)
        {
            var role = await roleRepository.GetByIdAsync(employee.RoleId);
            if (role == null)
                throw new Exception($"The Role ID {employee.RoleId} is invalid.");
            existingEmployee.RoleId = employee.RoleId;
            isUpdated = true;
        }

        if (isUpdated)
        {
            return await employeeWriteRepository.UpdateAsync(existingEmployee);
        }

        return existingEmployee;
    }

    public async Task<Employee> DeleteAsync(string id)
    {
        var employee = await employeeRepository.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            throw new Exception($"Employee with ID {id} not found.");
        }

        return await employeeWriteRepository.DeleteAsync(employee);
    }

    public async Task<Employee> SignInAsync(string id, string password)
    {
        var employee = await employeeRepository.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            throw new Exception($"Employee with ID {id} not found.");
        }
        else if (password != employee.Password)
        {
            throw new BadRequestException("Incorrect password.");
        }
        return employee;
    }

    #region Private Methods

    private string GenerateRandomId()
    {
        var random = new Random();
        var letters = new string(Enumerable.Range(0, 3)
                                           .Select(x => (char)random.Next('A', 'Z' + 1))
                                           .ToArray());

        var digits = random.Next(100, 1000);
        return $"LMS{letters}{digits}";
    }

    #endregion
}
