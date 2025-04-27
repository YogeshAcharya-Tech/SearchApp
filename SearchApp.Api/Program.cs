using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using SearchApp.Api.DependencyInjection;
using SearchApp.Api.MIddleware;
using SearchApp.Core;
using SearchApp.Core.Entities;
using SearchApp.Infrastructure.Repositories;
using System.Text;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);

// Configure Logging (Removes default Microsoft logging)
builder.Logging.ClearProviders();
builder.Host.UseNLog();

//Add services to the container.

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Bind appsettings.json from Configuration    
builder.Services.Configure<AppSettings>(builder.Configuration);

// Get values from appsettings.json
var jwtSettings = builder.Configuration.Get<AppSettings>() ?? new AppSettings();

//Added Code For JWT Authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    //options.SaveToken = true;
    //options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.JwtSetting.Issuer,
        ValidAudience = jwtSettings.JwtSetting.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.JwtSetting.SecretKey)),
        ClockSkew = TimeSpan.Zero // Optional: Adjust to reduce delay in token expiration
    };
});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SearchApp.API", Version = "v1" });
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAppDI();

builder.Services.AddSingleton<ICustomLogger, CustomLogger>();
builder.Services.AddSingleton<LogHelper>();
builder.Services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Seach.API v1"));
}

app.UseMiddleware<ApiResponseMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
