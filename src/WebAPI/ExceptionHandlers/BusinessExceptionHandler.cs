using Microsoft.AspNetCore.Diagnostics;
using WebAPI.Contracts;
using WebAPI.Exceptions;
using WebAPI.Exceptions.User;

namespace WebAPI.ExceptionHandlers;

public class BusinessExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BusinessException blException)
        {
            return false;
        }
        
        context.Response.StatusCode = GetExceptionStatusCode(blException);
        
        var errorResponse = new ErrorResponse(blException.ErrorCode, blException.ErrorMessage);
        await context.Response
            .WriteAsJsonAsync(errorResponse, cancellationToken);
        return true;
    }
    
    private static int GetExceptionStatusCode(Exception ex) => ex switch
    {
        NotFoundException => StatusCodes.Status404NotFound,
        ForbiddenException => StatusCodes.Status403Forbidden,
        UserAlreadyExistException => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status400BadRequest
    };
}