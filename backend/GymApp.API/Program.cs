using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GymApp.Infrastructure.Data;
using GymApp.Core.Interfaces;
using GymApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// CORS - allow all origins (temporary, to fix Vercel preview URLs)
// ==========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// ==========================
// Services
// ==========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================
// Database
// ==========================
var connectionString =
    Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ==========================
// Dependency Injection
// ==========================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICoachService, CoachService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IWorkoutPlanService, WorkoutPlanService>();
builder.Services.AddScoped<IDietPlanService, DietPlanService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IGoalService, GoalService>();

// ==========================
// JWT Authentication
// ==========================
var key = Encoding.ASCII.GetBytes(
    builder.Configuration["Jwt:Key"]
    ?? "SuperSecretKey123!@#$%"
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// ==========================
// Swagger (always on)
// ==========================
app.UseSwagger();
app.UseSwaggerUI();

// ==========================
// CORS - must be before auth
// ==========================
app.UseCors("AllowAll");

// ==========================
// OPTIONS handler (backup)
// ==========================
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization");
        await context.Response.CompleteAsync();
        return;
    }
    await next();
});

// app.UseHttpsRedirection(); // disabled for now
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();