using WebAPI.Contracts;

namespace WebAPI.Abstractions.Services;

public interface IUserService
{
    Task RegisterAsync(UserCreateRequest request, CancellationToken ct);

    Task<AuthenticationResponse> LoginAsync(UserLoginRequest request, CancellationToken ct);

    Task UpdateAsync(UserUpdateRequest request, CancellationToken ct);

    Task ChangePasswordAsync(UserChangePasswordRequest request, CancellationToken ct);

    Task<AuthenticationResponse> ChangeLoginAsync(UserChangeLoginRequest request, CancellationToken ct);

    Task<IEnumerable<UserResponse>> GetAllActiveAsync(GetUsersRequest request, CancellationToken ct);
    
    Task<UserResponse> GetByLoginAsync(string login, CancellationToken ct);

    Task<IEnumerable<UserResponse>> GetUsersOlderThanAsync(int age, CancellationToken ct);

    Task DeleteAsync(string login, bool isSoftDelete, CancellationToken ct);

    Task RestoreAsync(string login, CancellationToken ct);
}