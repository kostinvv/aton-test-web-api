using WebAPI.CustomAttributes;

namespace WebAPI.Contracts;

public record UserChangeLoginRequest
{
    public string Login { get; init; } = null!;
    
    [LatinLettersAndNumbers]
    public string NewLogin { get; init; } = null!;
};