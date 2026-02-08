using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Employees_WebAPI.Data;
using Employees_WebAPI.Mapping;
using Employees_WebAPI.Middleware;
using Employees_WebAPI.Repository;
using Employees_WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.OpenApi.Models;
using Employees_WebAPI.Filters;

namespace Employees_WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        //adding Logger
        builder.Host.UseSerilog();

        // Add services to the container.


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter here JWT Token with bearer format like"
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
             {
                 {
                      new OpenApiSecurityScheme
                      {
                          Reference = new OpenApiReference
                          {
                              Type = ReferenceType.SecurityScheme,
                              Id= "Bearer"
                          }
                      },

                       new string[]{ }
                  }
             });
        });

        //Add DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
                ));

        // Register application services and repositories for dependency injection
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IEmployeeRespository, EmployeeRepository>();

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile(new EmployeeProfile());
        });

        //adding Api Versioning
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
            .AddApiExplorer(options =>
        {
            options.SubstituteApiVersionInUrl = true;
            options.GroupNameFormat = "'v'VVV";
        });


        builder.Services.AddControllers();

        //Configuring JWT
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
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
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("HROnly", policy => policy.RequireClaim("Department", "HR"));

            options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));

            options.AddPolicy("AdminNHR", policy =>
            {
                policy.RequireClaim("Department", "HR", "Finance");
                policy.RequireRole("Admin");
            });
        });
        
        //Adding Action Method
        builder.Services.AddScoped<ActionLoggingFilter>();

        //Adding Custom Auth Filter
        builder.Services.AddScoped<CustomAuthFilter>();

        //Adding In-Memory Cache
        builder.Services.AddMemoryCache();

        //Adding Redis Cache
        builder.Services.AddStackExchangeRedisCache( option =>
        {
            option.Configuration = "localhost:6379";
            option.InstanceName = "EmployeesApi:";
        });

        var app = builder.Build();

        app.UseMiddleware<RequestLoggingMiddleware>();

        app.UseMiddleware<GlobalExceptionMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();


        app.MapControllers();

        //app.MapControllerRoute(
        //    name: "default",
        //    pattern: "api/{controller=Employees/{action=Get}/{id?}}");

        app.Run();
    }
}
