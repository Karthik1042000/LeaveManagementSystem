using LeaveManagementSystem.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.DB.Configuration
{
    public class LeaveApplicationConfiguration : IEntityTypeConfiguration<LeaveApplication>
    {
        public void Configure(EntityTypeBuilder<LeaveApplication> builder)
        {
            // Set the primary key
            builder.HasKey(la => la.Id);

            // Configure StartDate and EndDate properties as required
            builder.Property(la => la.StartDate)
                   .IsRequired();

            builder.Property(la => la.EndDate)
                   .IsRequired();

            // Configure LeaveType as required
            builder.Property(la => la.LeaveType)
                   .IsRequired();

            // Configure State as required
            builder.Property(la => la.State)
                   .IsRequired();

            // Define the relationship for Employee (who requested leave)
            builder.HasOne(la => la.Employee)
                   .WithMany()
                   .HasForeignKey(la => la.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict) // Disable cascade delete for EmployeeId
                   .IsRequired();

            // Define the relationship for Approver (who approved leave)
            builder.HasOne(la => la.Approver)
                   .WithMany()
                   .HasForeignKey(la => la.ApproverId)
                   .OnDelete(DeleteBehavior.Restrict);// Disable cascade delete
        }
    }
}
