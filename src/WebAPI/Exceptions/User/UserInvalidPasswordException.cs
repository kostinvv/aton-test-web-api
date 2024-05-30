namespace WebAPI.Exceptions.User;

public class UserInvalidPasswordException() : BusinessException("USER_INVALID_PASSWORD", Resources.ErrorMessage.USER_INVALID_PASSWORD);