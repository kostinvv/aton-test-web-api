using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions.Services;
using WebAPI.Contracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IUserService userService) : BaseController
{
    /// <summary>
    /// Аутентификация пользователя.
    /// </summary>
    /// <returns> Токен доступа. </returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] UserLoginRequest request, CancellationToken ct) 
        => Ok(await userService.LoginAsync(request, ct));
}