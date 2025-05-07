using MediatR;
using Application.Queries;
using Application.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace LIBSYSTEM.Endpoints
{
    public static class UsersEndpoint
    {
        public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/users", async (CreateUserCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Created($"api/users/{result.Id}", result);
            });

            app.MapGet("api/users/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetUserByIdQuery(id));
                return result is null ? Results.NotFound() : Results.Ok(result);
            });

            app.MapPut("api/users/{id}", async (int id, UpdateUserCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest();
                await sender.Send(command);
                return Results.NoContent();
            });

            app.MapGet("api/users/{id}/borrowed", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetBorrowedBooksByUserIdQuery(id));
                return Results.Ok(result);
            });

            // Librarian-only: View all users
            app.MapGet("/api/users", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllUsersQuery());
                return Results.Ok(result);
            }).RequireAuthorization("Librarian");

            // Librarian-only: View specific user
            app.MapGet("/api/users/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetUserByIdQuery(id));
                return Results.Ok(result);
            }).RequireAuthorization("Librarian");

            // Self-register
            app.MapPost("/api/users", async (CreateUserCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            });

            // Self-update
            app.MapPut("/api/users/{id}", async (int id, UpdateUserCommand command, ISender sender) =>
            {
                command.Id = id;
                await sender.Send(command);
                return Results.NoContent();
            }).RequireAuthorization();

            // Self-delete
            app.MapDelete("/api/users/{id}", async (int id, ISender sender) =>
            {
                await sender.Send(new DeleteUserCommand(id));
                return Results.NoContent();
            }).RequireAuthorization();
        }
    }
}