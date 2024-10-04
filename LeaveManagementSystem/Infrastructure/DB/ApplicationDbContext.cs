using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Infrastructure.DB.Configuration;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for entities
        public DbSet<Role> Role { get; set; } // Updated property name for consistency
        public DbSet<Employee> Employee { get; set; } // Updated property name for clarity
        public DbSet<LeaveApplication> LeaveApplication { get; set; } // Updated property name for clarity
        public DbSet<AnnualLeaveRecord> AnnualLeaveRecord { get; set; } // Updated property name for clarity
        public DbSet<LeaveUsageTracker> LeaveUsageTracker { get; set; } // Updated property name for clarity

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Auto loading the Role for Employee
            modelBuilder.Entity<Employee>()
                .Navigation(e => e.Role)
                .AutoInclude();

            // Auto loading the Employee for LeaveUsageTracker
            modelBuilder.Entity<LeaveUsageTracker>()
                .Navigation(lt => lt.Employee)
                .AutoInclude();

            // Auto loading the Role for AnnualLeaveRecord
            modelBuilder.Entity<AnnualLeaveRecord>()
                .Navigation(alr => alr.Role)
                .AutoInclude();

            // Auto loading Employee and Approver for LeaveApplication
            modelBuilder.Entity<LeaveApplication>()
                .Navigation(la => la.Employee)
                .AutoInclude();

            modelBuilder.Entity<LeaveApplication>()
                .Navigation(la => la.Approver)
                .AutoInclude();

            // Apply configurations for each entity
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveUsageTrackerConfiguration());
            modelBuilder.ApplyConfiguration(new AnnualLeaveRecordConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveApplicationConfiguration());
        }
    }
}
