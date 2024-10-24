namespace LagerhotellAPI.Models.CustomExceptionModels;

public class BadRequestException : Exception
{
    public BadRequestException(string message = null)
        : base(message == null ? "Bad Request" : message)
    { }
}

public class ActionInputIsNotValidException : BadRequestException
{
    public ActionInputIsNotValidException()
        : base("Action input is not valid")
    { }
}

public class InvalidVerificationCodeException : Exception
{
    public InvalidVerificationCodeException(string message = null)
        : base(message == null ? "Bad Request" : message)
    { }
}
public class TooManyRequestsException : Exception
{
    public TooManyRequestsException(string message = null)
        : base(message == null ? "Bad Request" : message)
    { }
}

