namespace WebAPI.Exceptions.User;

public class UserAlreadyExistException() : BusinessException("USER_ALREADY_EXIST", Resources.ErrorMessage.USER_ALREADY_EXIST);