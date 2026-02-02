
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Employees_WebAPI.Data;
using Employees_WebAPI.Mapping;
using Employees_WebAPI.Middleware;
using Employees_WebAPI.Repository;
using Employees_WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Employees_WebAPI
{
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
            builder.Services.AddSwaggerGen();

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

            app.UseAuthorization();


            app.MapControllers();

            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "api/{controller=Employees/{action=Get}/{id?}}");

            app.Run();
        }
    }
}
