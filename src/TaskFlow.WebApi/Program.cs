using TaskFlow.Infrastructure;
using TaskFlow.Application;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services
            // Add services to the container.
            builder.Services.AddAuthorization();

            // Add services to the other layers
            builder.Services
                .AddApplication()
                .AddInfrastructure();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();

            #region Endpoints

            // This endpoint retrieves a task by its ID.
            app.MapGet("/task/{id}", async (string id, IMediator mediator) =>
            {
                var result = await mediator.SendAsync(new GetTaskByIdQuery(Guid.Parse(id)));

                return result is not null
                ? Results.Ok(result)
                : Results.NotFound();
            })
            .WithName("GetTaskById")
            .WithOpenApi();

            // This endpoint retrieves all tasks or filters them based on the provided parameters.
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

            // This endpoint creates a new task based on the provided command.
            app.MapPost("/task", async ([FromBody]CreateTaskCommand command, IMediator mediator) =>
            {
                var id = await mediator.SendAsync(command);
                return Results.Ok(id);
            })
            .WithName("CreateTask")
            .WithOpenApi();

            //This endpoint delete a task by its ID.
            app.MapDelete("task/{id:guid}" , async (Guid id, IMediator mediator)=>
                {
                    return await mediator.SendAsync(new DeleteTaskCommand(id)) is not false
                      ? Results.Ok(id)
                      : Results.NotFound();
                }
            )
                .WithName("DeleteTask")
                .WithOpenApi();

            //This endpoint change the status of a task by its ID.
            app.MapPut("task/{id:guid}/status", async (Guid id, [FromBody] Domain.Enums.StatusEnum status, IMediator mediator) =>
            {
                return await mediator.SendAsync(new ChangeTaskStatusCommand(id, status)) is not false
                    ? Results.Ok(id)
                    : Results.NotFound();
            })
                .WithName("ChangeTaskStatus")
                .WithOpenApi();

            //This endpoint assign a task to a user by its ID.
            app.MapPut("task/{id:guid}/assign", async (Guid id, [FromBody] Guid userId, IMediator mediator) =>
            {
                return await mediator.SendAsync(new AssignTaskToUserCommand(id, userId)) is not false
                    ? Results.Ok(id)
                    : Results.NotFound();
            })
                .WithName("AssignTask")
                .WithOpenApi();

            #endregion


            app.Run();
        }
    }
}
