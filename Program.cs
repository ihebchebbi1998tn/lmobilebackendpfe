using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ConsolidatedApi.Data;
using ConsolidatedApi.Models;
using ConsolidatedApi.Services;
using ConsolidatedApi.Hubs;
using QuestPDF.Infrastructure;
using DotNetEnv;
using System.Security.Claims;

// Load environment variables
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Set QuestPDF license
QuestPDF.Settings.License = LicenseType.Community;

// Database configuration - Use Neon PostgreSQL
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
    "postgresql://neondb_owner:npg_CAiFLbX85sIq@ep-summer-fire-adwac3xi-pooler.c-2.us-east-1.aws.neon.tech/neondb?sslmode=require&channel_binding=require";

// Stripe configuration
var stripeSecretKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
if (!string.IsNullOrEmpty(stripeSecretKey))
{
    Stripe.StripeConfiguration.ApiKey = stripeSecretKey;
}

// Add services
builder.Services.AddHttpClient();

// Database context with PostgreSQL
builder.Services.AddDbContext<ConsolidatedDbContext>(options =>
    options.UseNpgsql(connectionString));

// Identity configuration
builder.Services.AddIdentity<ApplicationUser, Role>()
    .AddEntityFrameworkStores<ConsolidatedDbContext>()
    .AddDefaultTokenProviders();

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;
});

// JWT Authentication
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "your-super-secret-jwt-key-here";
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "ConsolidatedApi";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "ConsolidatedApi";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.NameIdentifier
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["accessToken"];
            return Task.CompletedTask;
        },
        OnChallenge = async context =>
        {
            if (!context.Handled && !context.Response.HasStarted)
            {
                var refreshToken = context.Request.Cookies["refreshToken"];
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(
                    string.IsNullOrEmpty(refreshToken)
                        ? "{\"message\":\"Unauthorized\"}"
                        : "{\"message\":\"Token expired\"}"
                );

                context.HandleResponse();
            }
        }
    };
});

// Register all services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<ClientOrganizationService>();
builder.Services.AddScoped<UiPageService>();
builder.Services.AddScoped<GeminiPromptService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<CustomerDevicesService>();
builder.Services.AddScoped<FeedbackService>();
builder.Services.AddScoped<InstallationRequestService>();
builder.Services.AddScoped<InvoicesService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ServiceRequestService>();
builder.Services.AddScoped<SparePartService>();
builder.Services.AddScoped<ServiceRequestAiService>();
builder.Services.AddScoped<StatsService>();
builder.Services.AddScoped<MinioService>();

// Add controllers and SignalR
builder.Services.AddControllers();
builder.Services.AddSignalR();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS configuration for production
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:3000";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                frontendUrl, 
                "http://localhost:3000", 
                "https://lmobileportal.vercel.app",
                "https://*.vercel.app",
                "https://*.onrender.com"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

// Configure URLs for Render deployment
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

// Database migration
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ConsolidatedDbContext>();

    logger.LogInformation("Migrating database...");
    try
    {
        await dbContext.Database.MigrateAsync();
        logger.LogInformation("Database migrated successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

// Configure API routes to match microservice structure
app.MapControllerRoute(
    name: "user-api",
    pattern: "user/api/{controller}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "service-api", 
    pattern: "service/api/{controller}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "chat-api",
    pattern: "chat/api/{controller}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "notification-api",
    pattern: "notification/api/{controller}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action=Index}/{id?}");

// Map SignalR hubs to match frontend expectations
app.MapHub<ChatHub>("/chat/chat/hub");
app.MapHub<UserToUserChatHub>("/chat/chat/UserToUser/hub");
app.MapHub<NotificationHub>("/notification/notification/hub");

// Health check endpoint is handled by HealthController

app.Run();