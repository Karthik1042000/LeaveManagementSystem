using LeaveManagementSystem.Domain.Events;
using LeaveManagementSystem.Infrastructure.Common;
using LeaveManagementSystem.Infrastructure.DB;
using LeaveManagementSystem.Infrastructure.EventHandlers;
using LeaveManagementSystem.Infrastructure.Exceptions;
using LeaveManagementSystem.Infrastructure.Interfaces;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;
using LeaveManagementSystem.Infrastructure.Repositories;
using LeaveManagementSystem.Infrastructure.Repositories.GenericRepositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System;

namespace LeaveManagementSystem.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection InfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add ApplicationDbContext with SQL Server configuration
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Register repositories (Generic and specific ones)
        services.AddAutoMapper(typeof(AnnualLeaveRecordMapper).Assembly);
        services.AddAutoMapper(typeof(ProfileMapper).Assembly);
        services.AddAutoMapper(typeof(EmployeeMapper).Assembly);
        services.AddAutoMapper(typeof(RoleMapper).Assembly);
        services.AddAutoMapper(typeof(LeaveApplicationMapper).Assembly);
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

        services.AddScoped<ILeaveService, LeaveService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IAnnualLeaveRecordRepository, AnnualLeaveRecordRepository>();
        services.AddScoped<ILeaveApplicationRepository, LeaveApplicationRepository>();
        services.AddScoped<ILeaveUsageTrackerRepository, LeaveUsageTrackerRepository>();
        
        services.AddControllers(options =>
        {
            options.Filters.Add<HttpResponseExceptionFilter>(); // Add the custom exception filter
        });

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Home/SignIn"; // Specify your login path
        });

        services.AddControllersWithViews();

        // Add HTTP context accessor and session services
        services.AddHttpContextAccessor();
        services.AddSession();

        // Register Event Handlers
        services.AddScoped<IEmployeeCreatedEventHandler, EmployeeCreatedEventHandler>();
        services.AddScoped<IAnnualLeaveRecordCreatedEventHandler, AnnualLeaveRecordCreatedEventHandler>();

        return services;
    }
}
