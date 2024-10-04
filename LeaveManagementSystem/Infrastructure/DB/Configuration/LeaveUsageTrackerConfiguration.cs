using LeaveManagementSystem.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.DB.Configuration
{
    public class LeaveUsageTrackerConfiguration : IEntityTypeConfiguration<LeaveUsageTracker>
    {
        public void Configure(EntityTypeBuilder<LeaveUsageTracker> builder)
        {
            // Define the primary key for the LeaveUsageTracker entity
            builder.HasKey(lt => lt.Id);

            // Define the relationship between LeaveUsageTracker and Employee
            builder.HasOne(lt => lt.Employee)
                   .WithMany()
                   .HasForeignKey(lt => lt.EmployeeId)
                   .IsRequired();

            // Configure the Year property as required
            builder.Property(lt => lt.Year)
                .IsRequired();

            builder.Property(lt => lt.ALUsed)
                .IsRequired();

            builder.Property(lt => lt.RHUsed)
                .IsRequired();

            builder.Property(lt => lt.BLUsed)
                .IsRequired();

            builder.Property(lt => lt.CLUsed)
                .IsRequired();

            // Define the table and check constraints
            builder.ToTable("LeaveUsageTracker", t =>
            {
                t.HasCheckConstraint("CK_LeaveUsageTracker_Year_Length", "[Year] >= 1000 AND [Year] <= 9999");
            });
        }
    }
}
