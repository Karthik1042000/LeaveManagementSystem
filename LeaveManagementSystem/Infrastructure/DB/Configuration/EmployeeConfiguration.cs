using LeaveManagementSystem.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.DB.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(25);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.RoleId)
                .IsRequired();

            builder.HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .IsRequired();

            builder.ToTable("Employee", t =>
            {
                t.HasCheckConstraint("CK_Name_MinLength", "LEN(Name) >= 3");
            });

            builder.ToTable("Employee", t =>
            {
                t.HasCheckConstraint("CK_Password_MinLength", "LEN(Password) >= 5");
            });

            builder.ToTable("Employee", t =>
            {
                t.HasCheckConstraint("CK_Email_Format", "[Email] LIKE '%@%.com'");
            });
        }
    }
}
