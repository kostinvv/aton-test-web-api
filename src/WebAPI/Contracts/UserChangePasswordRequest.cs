using System.Text.Json.Serialization;
using WebAPI.CustomAttributes;

namespace WebAPI.Contracts;

public record UserChangePasswordRequest
{
    [JsonIgnore] public string Login { get; init; } = string.Empty;
    
    public string CurrentPassword { get; init; } = null!;
    
    [LatinLettersAndNumbers]
    public string NewPassword { get; init; } = null!;
}