namespace AuthService.Shared;

public static class ErrorConstant
{
    public static int UnknownError = 1;

    // validation
    public const int InvalidRequest = 1000;
    public const int MissingIdentifier = 1001;

    // user
    public const int UserAlreadyExists = 2000;
    public const int UsernameAlreadyTaken = 2001;
    public const int EmailAlreadyUsed = 2002;
    public const int PhoneAlreadyUsed = 2003;
    public const int UserNotFound = 2004;

    // auth
    public const int InvalidPassword = 3000;
    public const int InvalidLogin = 3001;

    // confirmation
    public const int EmailNotConfirmed = 4000;
    public const int PhoneNotConfirmed = 4001;
    public const int InvalidEmailConfirmationCode = 4002;
    public const int InvalidPhoneConfirmationCode = 4003;

    // token
    public const int InvalidToken = 5000;
    public const int TokenExpired = 5001;
}
