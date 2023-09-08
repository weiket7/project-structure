namespace GetGo.Booking.Exceptions;

public class UnauthorizedException : Exception
{
    public string ErrorCode { get; set; }
    public string DisplayMessage { get; set; }

    public UnauthorizedException(string message) : base(message)
    {
    }
}