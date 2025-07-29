using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskFlow.Application;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;
using TaskFlow.Infrastructure;
using TaskFlow.Infrastructure.Auth;

namespace TaskFlow.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services

            //Add secrets depending on the environment
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }
            if (builder.Environment.IsProduction())
            {
                builder.Configuration.AddEnvironmentVariables();
            }

            // Add authentication and authorization services
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings!.Issuer,
                        ValidAudience = jwtSettings!.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.SecretKey))
                    };
                    options.RequireHttpsMetadata = true;
                });
            builder.Services.AddAuthorization();

            // Add services to the other layers
            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskFlow API", Version = "v1" });

                // Define el esquema de seguridad JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Ejemplo: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });



            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
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
                .RequireAuthorization()
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
                .RequireAuthorization()
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
                .RequireAuthorization()
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
                .RequireAuthorization()
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
                .RequireAuthorization()
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
                .RequireAuthorization()
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
                .RequireAuthorization()
                .WithName("GetUserById")
                .WithTags("User")
                .WithOpenApi();

            // This endpoint login an user in the application
            app.MapPost("/user/login", async (
                [FromBody] LoginCommand command,
                IMediator mediator,
                IValidator<LoginCommand> validator
                ) =>
                {
                    try
                    {
                        await validator.ValidateAndThrowAsync(command);
                        var jwt = await mediator.SendAsync(command);
                        return jwt is not null
                            ? Results.Ok(jwt)
                            : Results.Unauthorized();
                    }
                    catch (ValidationException e)
                    {
                        return Results.BadRequest(e.Errors);
                    }
                })
                .WithName("LoginUser")
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
                .RequireAuthorization()
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
                .RequireAuthorization()
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
                .RequireAuthorization()
                .WithName("DeleteGroup")
                .WithTags("Group")
                .WithOpenApi();

            // This endpoint updates a group.
            app.MapPut("/group", async (
                    [FromBody] UpdateGroupCommand command,
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
                .RequireAuthorization()
                .WithName("UpdateGroup")
                .WithTags("Group")
                .WithOpenApi();

            #endregion

            #endregion


            app.Run();
        }
    }
}
