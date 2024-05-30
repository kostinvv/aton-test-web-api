using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Abstractions.Context;

public interface IUserDbContext
{
    DbSet<User> Users { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken ct);
}