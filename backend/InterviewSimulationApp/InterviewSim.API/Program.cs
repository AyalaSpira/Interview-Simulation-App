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

var builder = WebApplication.CreateBuilder(args);

// === ????? CORS ===
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// === ????? DbContext ===
builder.Services.AddDbContext<InterviewSimContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// === ????? ???????? ?????? ?-DI ===
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService>(serviceProvider =>
{
    var bucketName = builder.Configuration["AWS:BucketName"]; // º??? ?-bucketName ???? º??? ?º??????????
    var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    var s3Service = serviceProvider.GetRequiredService<S3Service>();
    return new UserService(userRepository, s3Service, bucketName);
});
builder.Services.AddScoped<IInterviewService, InterviewService>();
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();

// ?????? ???? ????????
builder.Services.AddScoped<IAIService, AIService>();
builder.Services.AddHttpClient<AIService>();

// ?????? AWS S3
builder.Services.AddSingleton<S3Service>();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddHttpContextAccessor(); // ???? ???? ??

// ????? OpenAI API Key
var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];
if (string.IsNullOrEmpty(openAiApiKey))
{
    throw new Exception("OpenAI API Key is missing from configuration.");
}

// === ????? Authentication ???? JWT ===
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

// === ????? Controllers ===
builder.Services.AddControllers();

// === ????? Swagger ===
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

// === ????? ?????º??? ===
var app = builder.Build();

// === ????? ?????? ???? ??????? ===
using (var scope = app.Services.CreateScope())  // ???? ?? ?? ???? builder.Build()
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<InterviewSimContext>();
        dbContext.Database.Migrate(); // ????? ?-Migrations ??º?? EnsureCreated()
        Console.WriteLine("Database connection is successful!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error connecting to the database: {ex.Message}");
    }
}

// === ?????? Middleware ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// === ????? ?????º??? ===
app.Run();