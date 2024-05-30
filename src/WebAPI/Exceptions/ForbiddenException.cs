namespace WebAPI.Exceptions;

public class ForbiddenException() : BusinessException("FORBIDDEN", Resources.ErrorMessage.FORBIDDEN);