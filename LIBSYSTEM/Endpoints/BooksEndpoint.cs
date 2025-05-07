using MediatR;
using Application.Queries;
using Application.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace LIBSYSTEM.Endpoints   
{
    public static class BooksEndpoints
    {
        public static void MapBookEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/books", async ([FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetBooksQuery());
                return Results.Ok(result);
            });

            app.MapGet("api/books/{id}", async (int id, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetBookByIdQuery(id));
                return result is null ? Results.NotFound() : Results.Ok(result);
            });

            app.MapPost("api/books/borrow", async (BorrowBookCommand command, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            });

            app.MapPost("api/books/reserve", async (ReserveBookCommand command, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(command);
            });

            app.MapPost("api/books/return", async (ReturnBookCommand command, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            });

            app.MapPost("/api/books", async (AddBookCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            });

            app.MapPut("/api/books/{id}", async (int id, UpdateBookCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("Mismatched book ID.");
                await sender.Send(command);
                return Results.NoContent();
            });

            app.MapDelete("/api/books/{id}", async (int id, ISender sender) =>
            {
                await sender.Send(new DeleteBookCommand(id));
                return Results.NoContent();
            });
        }
    }
}
