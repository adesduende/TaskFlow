using TaskFlow.Infrastructure;
using TaskFlow.Application;

namespace TaskFlow.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Add services to the other layers
            builder.Services
                .AddInfrastructure()
                .AddApplication();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/task/{id}", (string id) =>
            {
                
                return true;
            })
            .WithName("GetTaskById")
            .WithOpenApi();

            app.MapGet("/task", () =>
            {

                return true;
            })
            .WithName("GetTasks")
            .WithOpenApi();

            app.Run();
        }
    }
}
