namespace WebAPI.Models;

public interface IAuditable
{
    DateTime CreatedOn { get; set; }
    string CreatedBy { get; set; }
    DateTime? ModifiedOn { get; set; }
    string? ModifiedBy { get; set; }
    DateTime? RevokedOn { get; set; }
    string? RevokedBy { get; set; }
}