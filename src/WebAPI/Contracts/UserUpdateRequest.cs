using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebAPI.CustomAttributes;
using WebAPI.Models;

namespace WebAPI.Contracts;

public record UserUpdateRequest
{
    [JsonIgnore] public string Login { get; init; } = string.Empty;
    
    [LatinAndCyrillicLetters]
    public string? Name { get; init; }
    
    [Range(0, 2)]
    public Gender? Gender { get; init; }
    
    public DateTime? Birthday { get; init; }
}