namespace UserService.Exceptions;

public class GeneralException : Exception
{
    public int Code { get; private set; }

    public GeneralException(int code, string message) : base(message)
    {
        Code = code;
    }
}