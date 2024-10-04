using LeaveManagementSystem.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.DB.Configuration
{
    public class LeaveUsageTrackerConfiguration : IEntityTypeConfiguration<LeaveUsageTracker>
    {
        public void Configure(EntityTypeBuilder<LeaveUsageTracker> builder)
        {
            builder.HasKey(lt => lt.Id);

            builder.HasOne(lt => lt.Employee)
                   .WithMany()
                   .HasForeignKey(lt => lt.EmployeeId)
                   .IsRequired();

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

            builder.ToTable("LeaveUsageTracker", t =>
            {
                t.HasCheckConstraint("CK_LeaveUsageTracker_Year_Length", "[Year] >= 1000 AND [Year] <= 9999");
            });
        }
    }
}
