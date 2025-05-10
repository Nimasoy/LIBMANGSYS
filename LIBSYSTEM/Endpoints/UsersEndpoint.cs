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
            // all-users access

            // add user
            app.MapPost("/api/users", async (CreateUserCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            }).WithTags("Users");
            // view user borrowed books
            app.MapGet("api/users/{id}/borrowed", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetBorrowedBooksByUserIdQuery(id));
                return Results.Ok(result);
            }).WithTags("Users");

            // librarian-only access

            // view all users
            app.MapGet("/api/librarian/users", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllUsersQuery());
                return Results.Ok(result);
            }).RequireAuthorization("Librarian").WithTags("Librarian - Users");
            // view user with id
            app.MapGet("/api/librarian/users/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetUserByIdQuery(id));
                return Results.Ok(result);
            }).RequireAuthorization("Librarian").WithTags("Librarian - Users");
            // update user
            app.MapPut("/api/librarian/users/{id}", async (int id, UpdateUserCommand command, ISender sender) =>
            {
                command.Id = id;
                await sender.Send(command);
                return Results.NoContent();
            }).RequireAuthorization("Librarian").WithTags("Librarian - Users");
            // delete user
            app.MapDelete("/api/librarian/users/{id}", async (int id, ISender sender) =>
            {
                await sender.Send(new DeleteUserCommand(id));
                return Results.NoContent();
            }).RequireAuthorization("Librarian").WithTags("Librarian - Users");
            // user borrowing history
            app.MapGet("api/users/{id}/borrowing-history", async (int id, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetUserBorrowingHistoryQuery(id));
                return Results.Ok(result);
            }).RequireAuthorization("Librarian").WithTags("Librarian - Users");
        }
    }
}