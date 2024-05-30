using WebAPI.Models;

namespace WebAPI.Contracts;

public record UserResponse(string Name, Gender Gender, DateTime? Birthday, bool IsActive);