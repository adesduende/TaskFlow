using TaskFlow.Infrastructure;
using TaskFlow.Application;
using TaskFlow.Application.DTO;
using TaskFlow.Application.UseCases.TaskCases;
using TaskFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
                .AddApplication()
                .AddInfrastructure();

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

            app.MapGet("/task/{id}", (string id, IMediator mediator) =>
            {                
                return true;
            })
            .WithName("GetTaskById")
            .WithOpenApi();

            app.MapGet("/tasks/", 
                async (
                    [FromQuery]Domain.Enums.StatusEnum? status,
                    [FromQuery] Domain.Enums.PriorityEnum? priority,
                    [FromQuery] Guid? user,
                    [FromQuery] DateTime? timeLimit,
                    [FromQuery] DateTime? createdAt , 
                    IMediator mediator
                    ) =>
            {
                // This endpoint retrieves tasks based on the provided parameters.
                IEnumerable<TaskDTO> tasks = new List<TaskDTO>();
                // If no parameters are provided, return all tasks
                if (status == null && priority == null && user == null && timeLimit == null && createdAt == null)
                    tasks = await mediator.SendAsync(new GetAllTasksQuery());
                else
                    tasks = await mediator.SendAsync(new GetAllTasksQuery(status, priority, user, timeLimit, createdAt));
                return tasks is not null
                    ? Results.Ok(tasks)
                    : Results.NotFound();
            })
            .WithName("GetTasks")
            .WithOpenApi();

            app.MapPost("/task", async ([FromBody]CreateTaskCommand command, IMediator mediator) =>
            {
                var id = await mediator.SendAsync(command);
                return Results.Ok(id);
            })
            .WithName("CreateTask")
            .WithOpenApi();


            app.Run();
        }
    }
}
