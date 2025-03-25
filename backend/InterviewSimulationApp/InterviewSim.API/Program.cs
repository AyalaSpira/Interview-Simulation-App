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

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
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

// === UserService ===
builder.Services.AddScoped<IUserService>(serviceProvider =>
{
    var bucketName = builder.Configuration["BucketName"];
    if (string.IsNullOrEmpty(bucketName))
    {
        throw new Exception("BucketName is missing from configuration.");
    }

    var accessKey = builder.Configuration["AccessKey"];
    var secretKey = builder.Configuration["SecretKey"];
    var region = builder.Configuration["Region"];
    var endpoint = builder.Configuration["Endpoint"];
    var model = builder.Configuration["Model"];

    var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    var s3Service = serviceProvider.GetRequiredService<S3Service>();
    var interviewRepository = serviceProvider.GetRequiredService<IInterviewRepository>();

    return new UserService(userRepository, interviewRepository, s3Service, bucketName);
});

// === AIService ===
builder.Services.AddScoped<IAIService, AIService>();
builder.Services.AddHttpClient<AIService>();

// === AWS S3 ===
builder.Services.AddSingleton<S3Service>();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddHttpContextAccessor();

// === OpenAI API Key ===
var openAiApiKey = builder.Configuration["ApiKey"];
if (string.IsNullOrEmpty(openAiApiKey))
{
    throw new Exception("OpenAI API Key is missing from configuration.");
}

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

// === Middleware ===
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
var url = $"http://0.0.0.0:{port}";
builder.WebHost.UseUrls(url);
builder.WebHost.UseUrls("http://0.0.0.0:" + port);
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<InterviewSimContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Database connection is successful!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error connecting to the database: {Message}", ex.Message);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
