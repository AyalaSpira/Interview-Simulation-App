using InterviewSim.DAL;
using InterviewSim.DAL.Repositories;
using InterviewSim.BLL.Interfaces;
using InterviewSim.BLL.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<InterviewSimContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()));
// הוספת השירותים לשירותי ה-DI
builder.Services.AddScoped<IUserRepository, UserRepository>();  // נוודא שה-Repository רשום
builder.Services.AddScoped<IAuthService, AuthService>();         // נוודא שה-AuthService רשום
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInterviewService, InterviewService>();
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();


// הוספת Swagger (OpenAPI) 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "InterviewSim API", Version = "v1" });
});

// בניית האפליקציה
var app = builder.Build();

// בדיקה אם ה-UserRepository נרשם כראוי
var scope = app.Services.CreateScope();
var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
Console.WriteLine(userRepository != null ? "UserRepository registered!" : "UserRepository not registered!");

// טיפול בשגיאות חיבור למסד נתונים
try
{
    var dbContext = scope.ServiceProvider.GetRequiredService<InterviewSimContext>();
    dbContext.Database.EnsureCreated(); // זה יוודא שהמסד נתונים קיים
    Console.WriteLine("Database connection is successful!");
}
catch (Exception ex)
{
    Console.WriteLine($"Error connecting to the database: {ex.Message}");
}

// קונפיגורציה של ה-HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "InterviewSim API v1");
    });
}

app.UseHttpsRedirection();

// קביעת ה-API Routes
app.MapGet("/weatherforecast", () =>
{
    string[] summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

// מודל לדוגמה (WeatherForecast) – השתמש במודלים שלך לפי הצורך
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
