using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using UserService.Dtos;
using UserService.Enums;

namespace UserService.Exceptions;

public class ErrorMiddleware
{
    private readonly RequestDelegate _Next;
    private static IHostEnvironment _Env;

    public ErrorMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        _Next = next;
        _Env = env;
    }

    public async Task Invoke(HttpContext context /* other dependencies */)
    {
        try
        {
            await _Next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        //sometimes there's AggregateException, need to get inner exception
        exception = exception.Get();

        //known exceptions
        if (exception is GeneralException)
        {
            Log.Error(exception.Message);
            Log.Error(exception.StackTrace);
        }
        else //unknown exceptions
        {
            Log.Fatal(exception.Message);
            Log.Fatal(exception.StackTrace);
        }

        var camelCaseSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        
        //for unknown exception, return generic message "An error occurred, please try again or contact live chat for assistance."
        var message = exception is GeneralException ? exception.Message : Message.InternalServerError;
        var code = exception is GeneralException generalException ? generalException.Code : (int)HttpStatusCode.InternalServerError;
        var result = JsonConvert.SerializeObject(ApiResponse.Error(code, message),
            camelCaseSerializerSettings);

        context.Response.ContentType = "application/json";
        
        var statusCode = HttpStatusCode.InternalServerError;
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(result);
    }
}