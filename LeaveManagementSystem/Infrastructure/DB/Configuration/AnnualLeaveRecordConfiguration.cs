using LeaveManagementSystem.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.DB.Configuration
{
    public class AnnualLeaveRecordConfiguration : IEntityTypeConfiguration<AnnualLeaveRecord>
    {
        public void Configure(EntityTypeBuilder<AnnualLeaveRecord> builder)
        {
            // Define the primary key for the AnnualLeaveRecord entity
            builder.HasKey(alr => alr.Id);

            // Configure the Year property as required
            builder.Property(alr => alr.Year)
                .IsRequired();

            // Configure leave properties (Annual Leave, Casual Leave, etc.)
            builder.Property(alr => alr.AnnualLeave)
                .IsRequired();

            builder.Property(alr => alr.CasualLeave)
                .IsRequired();

            builder.Property(alr => alr.RestrictedHoliday)
                .IsRequired();

            builder.Property(alr => alr.BonusLeave)
                .IsRequired();

            // Define the relationship between AnnualLeaveRecord and Role (many-to-one relationship)
            builder.HasOne(alr => alr.Role)
                .WithMany()
                .HasForeignKey(alr => alr.RoleId)
                .IsRequired();

            builder.ToTable("AnnualLeaveRecord", t =>
            {
                t.HasCheckConstraint("CK_AnnualLeaveRecord_Year_Length", "[Year] >= 1000 AND [Year] <= 9999");
            });
        }
    }
}
