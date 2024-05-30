namespace WebAPI.Exceptions;

public class NotFoundException() : BusinessException("NOT_FOUND", Resources.ErrorMessage.NOT_FOUND);