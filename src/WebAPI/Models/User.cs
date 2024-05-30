namespace WebAPI.Models;

public class User : IAuditable
{
    public Guid Id { get; init; }
    
    public string Login { get; set; } = null!;
    
    public string PasswordHash { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    public Gender Gender { get; set; }
    
    public DateTime? Birthday { get; set; }
    
    public bool IsAdmin { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public string CreatedBy { get; set; } = null!;
    
    public DateTime? ModifiedOn { get; set; }
    
    public string? ModifiedBy { get; set; }
    
    public DateTime? RevokedOn { get; set; }
    
    public string? RevokedBy { get; set; }
}