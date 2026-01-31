
using Employees_WebAPI.Data;
using Employees_WebAPI.Mapping;
using Employees_WebAPI.Middleware;
using Employees_WebAPI.Repository;
using Employees_WebAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace Employees_WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
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

            var app = builder.Build();

            app.UseMiddleware<RequestLoggingMiddleware>();

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
