namespace UserService.Dtos;

public class ApiResponse
{
    public int Code { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }

    public static ApiResponse Error(int code, string message, object data = null)
    {
        return new ApiResponse
        {
            Code = code,
            Message = message,
            Data = data,
        };
    }
}