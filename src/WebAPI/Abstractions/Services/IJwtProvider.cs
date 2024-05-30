using WebAPI.Models;

namespace WebAPI.Abstractions.Services;

public interface IJwtProvider
{
    string CreateAccessToken(User user);
}