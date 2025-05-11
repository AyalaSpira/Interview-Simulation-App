using Amazon.S3;
using InterviewSim.BLL.Implementations;
using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL;
using InterviewSim.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using MailKit;
using IMailService = InterviewSim.BLL.Interfaces.IMailService;
using InterviewSim.BLL.Services;
using Microsoft.Extensions.Logging;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

var builder = WebApplication.CreateBuilder(args);

// === בדיקת משתנים חסרים ===
void CheckConfiguration(string key)
{
    var value = builder.Configuration[key];
    if (string.IsNullOrEmpty(value))
    {
        throw new Exception($"Missing configuration key: {key}");
    }
}

// בדיקה למפתחות קריטיים
CheckConfiguration("Mail:SmtpServer");
CheckConfiguration("Mail:SmtpUsername");
CheckConfiguration("Mail:SmtpPassword");
CheckConfiguration("Mail:SmtpPort");
CheckConfiguration("OpenAI_ApiKey");  // שינוי לשם החדש
CheckConfiguration("OpenAI_Model");   // שינוי לשם החדש
CheckConfiguration("OpenAI_Endpoint"); // שינוי לשם החדש
CheckConfiguration("Jwt:Issuer");
CheckConfiguration("Jwt:Audience");
CheckConfiguration("Jwt:Key");
CheckConfiguration("AWS:BucketName");

Console.WriteLine("All required configurations are present.");

// === Mail Service ===
builder.Services.AddScoped<IMailService, SmtpMailService>(serviceProvider =>
{
    var smtpServer = builder.Configuration["Mail:SmtpServer"];
    var smtpUsername = builder.Configuration["Mail:SmtpUsername"];
    var smtpPassword = builder.Configuration["Mail:SmtpPassword"];
    var smtpPort = int.Parse(builder.Configuration["Mail:SmtpPort"]);

    return new SmtpMailService(smtpServer, smtpUsername, smtpPassword, smtpPort);
});

// === CORS ===
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy
//            .WithOrigins(
//                "http://localhost:3000", // React
//                "http://localhost:4200",
//               " https://interview-simulation-app-react.onrender.com"
//            )
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .AllowCredentials(); // אם את שולחת בקשות עם cookies או headers כמו Authorization
//    });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        builder =>
        {
            builder.WithOrigins(
                "http://localhost:3000", // React
                "http://localhost:4200", // Angular או כל פרויקט אחר
                "https://interview-simulation-app-react.onrender.com") // הכתובת של היישום שלך ברנדר
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});



// === DbContext ===
builder.Services.AddDbContext<InterviewSimContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// === Dependency Injection ===
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();
builder.Services.AddScoped<IInterviewService, InterviewService>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();

// === AWS Bucket Name ===
var bucketName = builder.Configuration["AWS:BucketName"];
builder.Services.AddScoped<IUserService>(provider =>
{
    var userRepository = provider.GetRequiredService<IUserRepository>();
    var interviewRepository = provider.GetRequiredService<IInterviewRepository>();
    var s3Service = provider.GetRequiredService<S3Service>();
    return new UserService(userRepository, interviewRepository, s3Service, bucketName);
});

// === AdminService ===
builder.Services.AddScoped<AdminService>(provider =>
{
    var userRepo = provider.GetRequiredService<IUserRepository>();
    var interviewRepo = provider.GetRequiredService<IInterviewRepository>();
    var s3Service = provider.GetRequiredService<S3Service>();
    var bucketName = builder.Configuration["AWS:BucketName"];
    return new AdminService(userRepo, interviewRepo, s3Service, bucketName);
});

// === AIService ===
builder.Services.AddScoped<IAIService, AIService>();
builder.Services.AddHttpClient<AIService>();

// === AWS S3 ===
builder.Services.AddSingleton<S3Service>();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddHttpContextAccessor();

// === JWT Authentication ===
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

// === Controllers ===
builder.Services.AddControllers();

// === Swagger ===
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "InterviewSim API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your token}' to authenticate"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// === Middleware ===
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080"; // ברירת מחדל אם PORT לא קיים
app.Urls.Add($"http://0.0.0.0:{port}");

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        var dbContext = services.GetRequiredService<InterviewSimContext>();
//        dbContext.Database.Migrate();
//        Console.WriteLine("Database connection is successful!");
//    }
//    catch (Exception ex)
//    {
//        var logger = services.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "Error connecting to the database: {Message}", ex.Message);
//    }
//}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI();
}

// === בדיקת משתני סביבה ===
var accessKey = Environment.GetEnvironmentVariable("AccessKey");
var secretKey = Environment.GetEnvironmentVariable("SecretKey"); 

if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
{
    Console.WriteLine("Warning: AWS credentials are missing or not set.");
}
else
{
    Console.WriteLine("AWS credentials are set correctly.");
}
app.UseCors("MyPolicy");

//app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
