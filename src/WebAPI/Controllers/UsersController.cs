using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions.Services;
using WebAPI.Contracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserService userService) : BaseController
{
    /// <summary>
    /// Создание нового пользователя.
    /// </summary>
    /// <returns> Токен доступа. </returns>
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] UserCreateRequest request, CancellationToken ct)
    {
        await userService.RegisterAsync(request, ct);
        return Created();
    }

    /// <summary>
    /// Обновление имени, пола или даты рождениея пользователя.
    /// </summary>
    /// <returns> Ok. </returns>
    [Authorize]
    [HttpPatch("{login}")]
    public async Task<IActionResult> UpdateAsync(
        string login, [FromBody] UserUpdateRequest request, CancellationToken ct)
    {
        request = request with { Login = login };
        await userService.UpdateAsync(request, ct);
        return Ok();
    }
    
    /// <summary>
    /// Обновление пароля пользователя.
    /// </summary>
    /// <returns> Ok. </returns>
    [Authorize]
    [HttpPatch("{login}/password")]
    public async Task<IActionResult> UpdateAsync(
        string login, [FromBody] UserChangePasswordRequest request, CancellationToken ct)
    {
        request = request with { Login = login };
        await userService.ChangePasswordAsync(request, ct);
        return Ok();
    }

    /// <summary>
    /// Обновление логина пользователя.
    /// </summary>
    /// <returns> Новый токен доступа. </returns>
    [Authorize]
    [HttpPatch("login")]
    public async Task<IActionResult> UpdateAsync(
        [FromQuery] UserChangeLoginRequest request, CancellationToken ct) 
        => Ok(await userService.ChangeLoginAsync(request, ct));
    
    /// <summary>
    /// Получение списка активных пользователей.
    /// </summary>
    /// <returns> Список активных пользователей. </returns>
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("active")]
    public async Task<IActionResult> GetAsync([FromQuery] GetUsersRequest request, CancellationToken ct)
        => Ok(await userService.GetAllActiveAsync(request, ct));
    
    /// <summary>
    /// Получение пользователя по логину.
    /// </summary>
    /// <returns> Пользователь. </returns>
    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] string login, CancellationToken ct) 
        => Ok(await userService.GetByLoginAsync(login, ct));

    /// <summary>
    /// Получение информации о текущем пользователе.
    /// </summary>
    /// <returns> Пользователь. </returns>
    [Authorize]
    [HttpGet("self")]
    public async Task<IActionResult> GetAsync(CancellationToken ct) 
        => Ok(await userService.GetByLoginAsync(Login, ct));

    /// <summary>
    /// Получение пользователей старше указанного возраста.
    /// </summary>
    /// <returns> Список пользователей. </returns>
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("age/{age:int}")]
    public async Task<IActionResult> GetAsync(int age, CancellationToken ct)
        => Ok(await userService.GetUsersOlderThanAsync(age, ct)); 

    /// <summary>
    /// Удаление пользователя полное или мягкое.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{login}")]
    public async Task<IActionResult> DeleteAsync(string login, [FromQuery] bool isSoftDelete, CancellationToken ct)
    {
        await userService.DeleteAsync(login, isSoftDelete, ct);
        return NoContent();
    }

    /// <summary>
    /// Восстановление пользователя.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpPatch("{login}/restore")]
    public async Task<IActionResult> RestoreAsync(string login, CancellationToken ct)
    {
        await userService.RestoreAsync(login, ct);
        return NoContent();
    }
}