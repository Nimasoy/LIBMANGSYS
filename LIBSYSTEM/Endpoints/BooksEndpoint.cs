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

            // all-users access

            // get all books
            app.MapGet("api/books", async ([FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetBooksQuery());
                return Results.Ok(result);
            }).WithTags("Books");
            // get a book by id
            app.MapGet("api/books/{id}", async (int id, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetBookByIdQuery(id));
                return result is null ? Results.NotFound() : Results.Ok(result);
            }).WithTags("Books");
            // borrow a book
            app.MapPost("api/books/borrow", async (BorrowBookCommand command, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            }).WithTags("Books");
            // reserve a book
            app.MapPost("api/books/reserve", async (ReserveBookCommand command, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            }).WithTags("Books");
            // return a book
            app.MapPost("api/books/return", async (ReturnBookCommand command, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            }).WithTags("Books");
            // user overdue
            app.MapGet("api/books/overdue/{userId}", async (int userId, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetUserOverdueBooksQuery(userId));
                return Results.Ok(result);
            }).RequireAuthorization().WithTags("Books");

            // librarian-only access

            // add a book 
            app.MapPost("/api/librarian/books", async (AddBookCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            }).RequireAuthorization("Librarian").WithTags("Librarian - Books");
            // update a book 
            app.MapPut("/api/librarian/books/{id}", async (int id, UpdateBookCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("Mismatched book ID.");
                await sender.Send(command);
                return Results.NoContent();
            }).RequireAuthorization("Librarian").WithTags("Librarian - Books");
            // delete a book
            app.MapDelete("/api/librarian/books/{id}", async (int id, ISender sender) =>
            {
                await sender.Send(new DeleteBookCommand(id));
                return Results.NoContent();
            }).RequireAuthorization("Librarian").WithTags("Librarian - Books");
            // overdue
            app.MapGet("api/books/overdue", async ([FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetOverdueBooksQuery());
                return Results.Ok(result);
            }).RequireAuthorization("Librarian").WithTags("Librarian - Books");

            
        }
    }
}
