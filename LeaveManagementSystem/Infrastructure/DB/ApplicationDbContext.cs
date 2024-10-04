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
        public DbSet<Role> Role { get; set; } 
        public DbSet<Employee> Employee { get; set; } 
        public DbSet<LeaveApplication> LeaveApplication { get; set; } 
        public DbSet<AnnualLeaveRecord> AnnualLeaveRecord { get; set; } 
        public DbSet<LeaveUsageTracker> LeaveUsageTracker { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .Navigation(e => e.Role)
                .AutoInclude();

            modelBuilder.Entity<LeaveUsageTracker>()
                .Navigation(lt => lt.Employee)
                .AutoInclude();

            modelBuilder.Entity<AnnualLeaveRecord>()
                .Navigation(alr => alr.Role)
                .AutoInclude();

            modelBuilder.Entity<LeaveApplication>()
                .Navigation(la => la.Employee)
                .AutoInclude();

            modelBuilder.Entity<LeaveApplication>()
                .Navigation(la => la.Approver)
                .AutoInclude();

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveUsageTrackerConfiguration());
            modelBuilder.ApplyConfiguration(new AnnualLeaveRecordConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveApplicationConfiguration());
        }
    }
}
