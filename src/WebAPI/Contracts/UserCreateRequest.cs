using System.ComponentModel.DataAnnotations;
using WebAPI.CustomAttributes;
using WebAPI.Models;

namespace WebAPI.Contracts;

public record UserCreateRequest
{
    [LatinLettersAndNumbers]
    public string Login { get; init; } = null!;
    
    [LatinLettersAndNumbers]
    public string Password { get; init; } = null!;
    
    [LatinAndCyrillicLetters]
    public string Name { get; init; } = null!;
    
    [Range(0, 2)]
    public Gender Gender { get; init; } 
    
    public DateTime? Birthday { get; init; }
    
    public bool IsAdmin { get; init; }
}