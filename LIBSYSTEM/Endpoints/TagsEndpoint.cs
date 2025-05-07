using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace LIBSYSTEM.Endpoints
{
    public static class TagsEndpoint
    {
        public static void MapTagsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/tags", async (CreateTagCommand command, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            });

            app.MapDelete("/api/tags/{id}", async (int id, [FromServices] ISender sender) =>
            {
                await sender.Send(new DeleteTagCommand(id));
                return Results.NoContent();
            });

        }
    }
}
