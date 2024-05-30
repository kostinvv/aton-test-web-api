using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebAPI.Abstractions.Context;
using WebAPI.Abstractions.Services;
using WebAPI.Contracts;
using WebAPI.Exceptions;
using WebAPI.Exceptions.User;
using WebAPI.Models;

namespace WebAPI.Services;

public class UserService(
    IUserDbContext context, 
    IJwtProvider jwtProvider, 
    IHttpContextAccessor httpContextAccessor) : IUserService
{
    public async Task RegisterAsync(UserCreateRequest request, CancellationToken ct)
    {
        var user = await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(user => user.Login == request.Login, cancellationToken: ct);

        if (user is not null)
        {
            throw new UserAlreadyExistException();
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user = new User
        {
            Login = request.Login,
            PasswordHash = passwordHash,
            Name = request.Name,
            Gender = request.Gender,
            Birthday = request.Birthday,
            IsAdmin = request.IsAdmin,
        };

        await context.Users.AddAsync(user, cancellationToken: ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<AuthenticationResponse> LoginAsync(UserLoginRequest request, CancellationToken ct)
    {
        var user = await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(user => user.Login == request.Login, cancellationToken: ct);
        
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UserLoginException();
        }

        if (user.RevokedOn is not null)
        {
            throw new UserRevokedException();
        }
        
        var accessToken = jwtProvider.CreateAccessToken(user);
        var response = new AuthenticationResponse(accessToken);
        return response;
    }

    public async Task UpdateAsync(UserUpdateRequest request, CancellationToken ct)
    {
        var user = await GetUserOrThrowAsync(request.Login, ct);

        ValidateUserAccess(user);

        user.Name = request.Name ?? user.Name;
        user.Gender = request.Gender ?? user.Gender;
        user.Birthday = request.Birthday ?? user.Birthday;

        context.Users.Update(user);
        await context.SaveChangesAsync(ct);
    }

    public async Task ChangePasswordAsync(UserChangePasswordRequest request, CancellationToken ct)
    {
        var user = await GetUserOrThrowAsync(request.Login, ct);
        
        ValidateUserAccess(user);
        
        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
        {
            throw new UserInvalidPasswordException();
        }
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        
        context.Users.Update(user);
        await context.SaveChangesAsync(ct);
    }

    public async Task<AuthenticationResponse> ChangeLoginAsync(UserChangeLoginRequest request, CancellationToken ct)
    {
        var user = await GetUserOrThrowAsync(request.Login, ct);

        ValidateUserAccess(user);
        
        var existUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == request.NewLogin, ct);

        if (existUser is not null)
        {
            throw new UserAlreadyExistException();
        }

        user.Login = request.NewLogin;
        
        context.Users.Update(user);
        await context.SaveChangesAsync(ct);
        
        var token = jwtProvider.CreateAccessToken(user);
        var response = new AuthenticationResponse(token);
        
        return response;
    }

    public async Task<IEnumerable<UserResponse>> GetAllActiveAsync(GetUsersRequest request, CancellationToken ct)
    {
        var query = context.Users
            .AsNoTracking()
            .Where(user => user.RevokedOn == null);

        query = request.SortOrder == "desc"
            ? query.OrderByDescending(user => user.CreatedOn) 
            : query.OrderBy(user => user.CreatedOn);

        var response = await query.Select(user =>
            new UserResponse(user.Name, user.Gender, user.Birthday, user.RevokedOn == null))
            .ToListAsync(cancellationToken: ct);

        return response;
    }

    public async Task<UserResponse> GetByLoginAsync(string login, CancellationToken ct)
    {
        var user = await GetUserOrThrowAsync(login, ct);
        
        var isActive = user.RevokedOn == null;
        var response = new UserResponse(user.Name, user.Gender, user.Birthday, isActive);

        return response;
    }

    public async Task<IEnumerable<UserResponse>> GetUsersOlderThanAsync(int age, CancellationToken ct) 
        => await context.Users.Where(user => user.Birthday <= DateTime.UtcNow.AddYears(-age))
            .Select(user => new UserResponse(user.Login, user.Gender, user.Birthday, user.RevokedOn == null))
            .ToListAsync(ct);

    public async Task DeleteAsync(string login, bool isSoftDelete, CancellationToken ct)
    {
        var user = await GetUserOrThrowAsync(login, ct);

        var httpContext = httpContextAccessor.HttpContext!;
        
        if (isSoftDelete)
        {
            user.RevokedOn = DateTime.UtcNow;
            user.RevokedBy = httpContext.User.FindFirstValue(ClaimTypes.Name);

            context.Users.Update(user);
        }
        else
        {
            context.Users.Remove(user);
        }

        await context.SaveChangesAsync(ct);
    }

    public async Task RestoreAsync(string login, CancellationToken ct)
    {
        var user = await GetUserOrThrowAsync(login, ct);

        user.RevokedOn = null;
        user.RevokedBy = null;

        context.Users.Update(user);
        await context.SaveChangesAsync(ct);
    }

    private async Task<User> GetUserOrThrowAsync(string login, CancellationToken ct)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Login == login, ct);

        if (user is null)
        {
            throw new NotFoundException();
        }

        return user;
    }
    
    private void ValidateUserAccess(User user)
    {
        if (user.RevokedOn is not null)
        {
            throw new UserRevokedException();
        }

        var httpContext = httpContextAccessor.HttpContext!;
        var isCurrentUser = httpContext.User.FindFirstValue(ClaimTypes.Name) == user.Login;
        var isAdmin = httpContext.User.FindFirstValue(ClaimTypes.Role) == "Admin";
        
        if (!isAdmin && !isCurrentUser)
        {
            throw new ForbiddenException();
        }
    }
}