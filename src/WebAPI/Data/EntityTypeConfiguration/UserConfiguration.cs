using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Models;

namespace WebAPI.Data.EntityTypeConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(user => user.Id);
        
        builder.HasIndex(user => user.Login)
            .IsUnique();

        builder.Property(user => user.Id)
            .HasColumnName("user_id");

        builder.Property(user => user.PasswordHash)
            .IsRequired()
            .HasColumnName("password_hash");
        
        builder.Property(user => user.Login)
            .IsRequired()
            .HasColumnName("login")
            .HasMaxLength(255);
        
        builder.Property(user => user.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(255);

        builder.Property(user => user.Gender)
            .IsRequired()
            .HasColumnName("gender");
        
        builder.Property(user => user.Birthday)
            .HasColumnName("birthday_date");
        
        builder.Property(user => user.IsAdmin)
            .HasColumnName("is_admin");
        
        builder.Property(user => user.CreatedBy)
            .IsRequired()
            .HasColumnName("created_by")
            .HasMaxLength(255);
        
        builder.Property(user => user.CreatedBy)
            .IsRequired()
            .HasColumnName("created_by")
            .HasMaxLength(255);
        
        builder.Property(user => user.ModifiedBy)
            .HasColumnName("modified_by")
            .HasMaxLength(255);
        
        builder.Property(user => user.RevokedBy)
            .HasColumnName("revoked_by")
            .HasMaxLength(255);

        builder.Property(user => user.CreatedOn)
            .HasColumnName("created_on");
        
        builder.Property(user => user.ModifiedOn)
            .HasColumnName("modified_on");
        
        builder.Property(user => user.RevokedOn)
            .HasColumnName("revoked_on");
    }
}