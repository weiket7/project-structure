using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UserService.Apis;
using UserService.Exceptions;
using UserService.Helpers;
using UserService.Models;

var builder = WebApplication.CreateBuilder(args);

//ConfigureDatabase()
var connectionString = builder.Configuration.Get<AppSettings>().DatabaseConnection;
if (string.IsNullOrEmpty(connectionString))
{
    throw new GeneralException("ConfigureDatabase - Connection string is empty");
}

builder.Services.AddDbContextPool<DatabaseContext>((serviceProvider, options) =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        );
    })
);

//RegisterDependencies
builder.Services.AddScoped<ApiLoggingHandler>();

builder.Services.AddHttpClient<IKycApi, KycApi>().AddHttpMessageHandler<ApiLoggingHandler>();

//ConfigureLogging()
builder.Host.UseSerilog((ctx, lc) => lc
    //for Microsoft / .NET Core logs, only warning level and above will be logged, meaning information level will not be logged
    //any _logger.LogInformation("") will still be logged as per normal
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console());

//EnableValidateRequestAttribute();
// enable [ValidateRequest] attribute on controllers and end points
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware(typeof(ErrorMiddleware));

app.MapControllers();

app.Run();

public class AppSettings
{
    [ConfigurationKeyName("database_connection")]
    public string DatabaseConnection { get; set; }
}

public class GeneralException : Exception
{
    public GeneralException(string message)
    {
        
    }
    
    public GeneralException(int code, string message)
    {
        
    }
}
