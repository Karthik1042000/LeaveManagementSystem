using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Domain;

namespace LeaveManagementSystem.Infrastructure.DB.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);

            builder.Property(u => u.State)
            .IsRequired();

            builder.ToTable("Role", t =>
            {
                t.HasCheckConstraint("CK_Name_MinLength", "LEN(Name) >= 3");
            });
        }
    }
}
