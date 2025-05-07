using MediatR;
using Application.Queries;
using Application.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace LIBSYSTEM.Endpoints
{
    public static class CategoriesEndpoint
    {
        public static void MapCategoriesEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/categories", async ([FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetCategoriesQuery());
                return Results.Ok(result);
            });

            app.MapPost("/api/categories", async (CreateCategoryCommand command, [FromServices] ISender sender) =>
            {
                await sender.Send(command);
                return Results.Ok();
            });

            app.MapPut("/api/categories/{id}", async (int id, UpdateCategoryCommand command, [FromServices] ISender sender) =>
            {
                command.Id = id;
                await sender.Send(command);
                return Results.Ok();
            });

            app.MapDelete("/api/categories/{id}", async (int id, [FromServices] ISender sender) =>
            {
                await sender.Send(new DeleteCategoryCommand { Id = id });
                return Results.Ok();
            });


        }
    }
}