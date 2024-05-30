using System.Reflection;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebAPI.Abstractions.Context;
using WebAPI.Models;

namespace WebAPI.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor) 
    : DbContext(options), IUserDbContext
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<User>().HasData(new User
        {
            Id = Guid.NewGuid(),
            Login = "admin",
            Name = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
            Gender = Gender.Unknown,
            IsAdmin = true,
            CreatedBy = "admin",
        });
        
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker.Entries<IAuditable>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified);

        var context = httpContextAccessor.HttpContext;
        var login = context?.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        
        foreach (var entry in entries)
        {
            if (entry.State is EntityState.Added)
            {
                entry.Property(auditable => auditable.CreatedOn).CurrentValue = DateTime.UtcNow;
                entry.Property(auditable => auditable.CreatedBy).CurrentValue = login;
            }
            else if (entry.State is EntityState.Modified)
            {
                entry.Property(auditable => auditable.ModifiedOn).CurrentValue = DateTime.UtcNow;
                entry.Property(auditable => auditable.ModifiedBy).CurrentValue = login;
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}