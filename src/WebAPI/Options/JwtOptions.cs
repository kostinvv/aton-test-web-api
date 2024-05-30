namespace WebAPI.Options;

public class JwtOptions
{
    public string SecretKey { get; init; } = null!;
    
    public string Audience { get; init; } = null!;
    
    public string Issuer { get; init; } = null!;
    
    public int ExpiresHours { get; init; } 
}