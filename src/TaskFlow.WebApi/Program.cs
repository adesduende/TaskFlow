using TaskFlow.Infrastructure;
using TaskFlow.Application;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

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

            #region Task Endpoints
            // This endpoint retrieves a task by its ID.
            app.MapGet("/task/{id}", async (string id, IMediator mediator) =>
            {
                var result = await mediator.SendAsync(new GetTaskByIdQuery(Guid.Parse(id)));

                return result is not null
                ? Results.Ok(result)
                : Results.NotFound();
            })
                .WithName("GetTaskById")
                .WithTags("Task")
                .WithOpenApi();

            // This endpoint retrieves all tasks or filters them based on the provided parameters.
            app.MapGet("/tasks/",
                async (
                    [FromQuery] Domain.Enums.StatusEnum? status,
                    [FromQuery] Domain.Enums.PriorityEnum? priority,
                    [FromQuery] Guid? user,
                    [FromQuery] DateTime? timeLimit,
                    [FromQuery] DateTime? createdAt,
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
                .WithTags("Task")
                .WithOpenApi();

            // This endpoint creates a new task based on the provided command.
            app.MapPost("/task", async (
                [FromBody] CreateTaskCommand command,
                IMediator mediator,
                IValidator<CreateTaskCommand> validator) =>
            {
                try
                {
                    await validator.ValidateAndThrowAsync(command);
                    var res = await mediator.SendAsync(command);
                    return Results.Ok(res);

                }
                catch (ValidationException e)
                {

                    return Results.BadRequest(e.Errors);
                }

            })
                .WithName("CreateTask")
                .WithTags("Task")
                .WithOpenApi();

            //This endpoint delete a task by its ID.
            app.MapDelete("task/{id:guid}", async (Guid id, IMediator mediator) =>
                {
                    return await mediator.SendAsync(new DeleteTaskCommand(id)) is not false
                      ? Results.Ok(id)
                      : Results.NotFound();
                }
            )
                .WithName("DeleteTask")
                .WithTags("Task")
                .WithOpenApi();

            //This endpoint change the status of a task by its ID.
            app.MapPut("task/{id:guid}/status", async (Guid id, [FromBody] Domain.Enums.StatusEnum status, IMediator mediator) =>
            {
                return await mediator.SendAsync(new ChangeTaskStatusCommand(id, status)) is not false
                    ? Results.Ok(id)
                    : Results.NotFound();
            })
                .WithName("ChangeTaskStatus")
                .WithTags("Task")
                .WithOpenApi();

            //This endpoint assign a task to a user by its ID.
            app.MapPut("task/{id:guid}/assign", async (Guid id, [FromBody] Guid userId, IMediator mediator) =>
            {
                return await mediator.SendAsync(new AssignTaskToUserCommand(id, userId)) is not false
                    ? Results.Ok(id)
                    : Results.NotFound();
            })
                .WithName("AssignTask")
                .WithTags("Task")
                .WithOpenApi();
            #endregion

            #region User Endpoints
            // This endpoint retrieves a user by its ID.
            app.MapGet("/user/{id:guid}", async (
                Guid id,
                IMediator mediator,
                IValidator<GetUserByIdQuery> validator
                ) =>
                {
                    try
                    {
                        var command = new GetUserByIdQuery(id);
                        await validator.ValidateAndThrowAsync(command);
                        var res = await mediator.SendAsync(command);

                        return res is not null
                            ? Results.Ok(res)
                            : Results.NotFound();
                    }
                    catch (ValidationException e)
                    {
                        return Results.BadRequest(e.Errors);
                    }

                })
                .WithName("GetUserById")
                .WithTags("User")
                .WithOpenApi();

            // This endpoint creates a new user based on the provided command.
            app.MapPost("/user", async (
                    [FromBody] CreateUserCommand command,
                    IMediator mediator,
                    IValidator<CreateUserCommand> validator
                ) =>
                {
                    try
                    {
                        await validator.ValidateAndThrowAsync(command);
                        var userId = await mediator.SendAsync(command);
                        return Results.Ok(userId);
                    }
                    catch (ValidationException e)
                    {
                        return Results.BadRequest(e.Errors);
                    }
                })
                .WithName("CreateUser")
                .WithTags("User")
                .WithOpenApi();

            #endregion

            #region Group Endpoints

            // This endpoint creates a new group based on the provided command.
            app.MapPost("/group", async (
                    [FromBody] CreateGroupCommand command,
                    IMediator mediator,
                    IValidator<CreateGroupCommand> validator
                ) =>
                {
                    try
                    {
                        await validator.ValidateAndThrowAsync(command);
                        var groupId = await mediator.SendAsync(command);
                        return Results.Ok(groupId);
                    }
                    catch (ValidationException e)
                    {
                        return Results.BadRequest(e.Errors);
                    }
                })
                .WithName("CreateGroup")
                .WithSummary("Create Group")
                .WithDescription("Creates a new group with the specified details.")
                .WithTags("Group")
                .WithOpenApi();

            // This endpoint retrieves a group by its ID.
            app.MapGet("/group/{id:guid}", async (
                    Guid id,
                    IMediator mediator,
                    IValidator<GetGroupByIdQuery> validator
                ) =>
                {
                    try
                    {
                        var command = new GetGroupByIdQuery(id);
                        await validator.ValidateAndThrowAsync(command);
                        var res = await mediator.SendAsync(command);
                        return res is not null
                            ? Results.Ok(res)
                            : Results.NotFound();
                    }
                    catch (ValidationException e)
                    {
                        return Results.BadRequest(e.Errors);
                    }

                })
                .WithName("GetGroupById")
                .WithTags("Group")
                .WithOpenApi();

            //This endpoint deletes a group by its ID.
            app.MapDelete("/group/{id:guid}", async (
                Guid id, 
                IMediator mediator,
                IValidator<DeleteGroupCommand> validator
                ) =>
                {
                    try 
                    {
                        var command = new DeleteGroupCommand(id);
                        await validator.ValidateAndThrowAsync(command);
                        return await mediator.SendAsync(command) is not false
                            ? Results.Ok(id)
                            : Results.NotFound();
                    }
                    catch (ValidationException e)
                    {
                        return Results.BadRequest(e.Errors);
                    }
                })
                .WithName("DeleteGroup")
                .WithTags("Group")
                .WithOpenApi();

            // This endpoint updates a group.
            app.MapPut("/group", async(
                    [FromBody]UpdateGroupCommand command,
                    IMediator mediator,
                    IValidator<UpdateGroupCommand> validator
                ) =>
                { 
                    try
                    {
                        await validator.ValidateAndThrowAsync(command);
                        var res = await mediator.SendAsync(command);
                        return Results.Ok(res);
                    }
                    catch (ValidationException e)
                    {
                        return Results.BadRequest(e.Errors);
                    }
                })
                .WithName("UpdateGroup")
                .WithTags("Group")
                .WithOpenApi();

            #endregion

            #endregion


            app.Run();
        }
    }
}
