using MediatR;
using Application.Queries;
using Application.Commands;
using Microsoft.AspNetCore.Mvc;
namespace LIBSYSTEM.Endpoints
{
    public static class CategoriesEndpoint
    {
        public static void MapCategoriesEndpoints(this IEndpointRouteBuilder app)
        {
            // all-users access

            // get all categories
            app.MapGet("api/categories", async ([FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetCategoriesQuery());
                return Results.Ok(result);
            }).WithTags("Categories");

            // librarian-only access
            // add a category
            app.MapPost("/api/librarian/categories", async (CreateCategoryCommand command, [FromServices] ISender sender) =>
            {
                await sender.Send(command);
                return Results.Ok();
            }).RequireAuthorization("Librarian").WithTags("Librarian - Categories");
            // update a category
            app.MapPut("/api/librarian/categories/{id}", async (int id, UpdateCategoryCommand command, [FromServices] ISender sender) =>
            {
                command.Id = id;
                await sender.Send(command);
                return Results.Ok();
            }).RequireAuthorization("Librarian").WithTags("Librarian - Categories");
            // delete a category
            app.MapDelete("/api/librarian/categories/{id}", async (int id, [FromServices] ISender sender) =>
            {
                await sender.Send(new DeleteCategoryCommand { Id = id });
                return Results.Ok();
            }).RequireAuthorization("Librarian").WithTags("Librarian - Categories");
        }
    }
}