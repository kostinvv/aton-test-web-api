namespace WebAPI.Exceptions;

public class BusinessException(string errorCode, string errorMessage) : Exception(errorMessage)
{
    public string ErrorCode => errorCode;

    public string ErrorMessage => Message;
}