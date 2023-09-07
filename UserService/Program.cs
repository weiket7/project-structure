using Microsoft.EntityFrameworkCore;
using UserService.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.Get<AppSettings>().DatabaseConnection;
if (string.IsNullOrEmpty(connectionString))
{
    throw new GeneralException("Connection string is empty");
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
