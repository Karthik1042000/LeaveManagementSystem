using LeaveManagementSystem.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.DB.Configuration
{
    public class LeaveApplicationConfiguration : IEntityTypeConfiguration<LeaveApplication>
    {
        public void Configure(EntityTypeBuilder<LeaveApplication> builder)
        {
            builder.HasKey(la => la.Id);

            builder.Property(la => la.StartDate)
                   .IsRequired();

            builder.Property(la => la.EndDate)
                   .IsRequired();

            builder.Property(la => la.LeaveType)
                   .IsRequired();

            builder.Property(la => la.State)
                   .IsRequired();

            builder.HasOne(la => la.Employee)
                   .WithMany()
                   .HasForeignKey(la => la.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict) 
                   .IsRequired();

            builder.HasOne(la => la.Approver)
                   .WithMany()
                   .HasForeignKey(la => la.ApproverId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
