namespace WebAPI.Exceptions.User;

public class UserRevokedException() : BusinessException("USER_REVOKED", Resources.ErrorMessage.USER_REVOKED);