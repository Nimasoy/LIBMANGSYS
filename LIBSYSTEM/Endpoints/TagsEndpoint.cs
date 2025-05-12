using Application.Commands.Tags;
using Application.Queries.Tags;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace LIBSYSTEM.Endpoints
{
    public static class TagsEndpoint
    {
        public static void MapTagsEndpoints(this IEndpointRouteBuilder app)
        {
            // librarian-access only

            app.MapGet("/api/tags", async (ISender sender) =>
            {
                var result = await sender.Send(new GetTagsQuery());
                return Results.Ok(result);
            }).RequireAuthorization("Librarian").WithTags("Librarian - Tags");

            // add a tag
            app.MapPost("/api/librarian/tags", async (CreateTagCommand command, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            }).RequireAuthorization("Librarian").WithTags("Librarian - Tags");
            // remove a tag
            app.MapDelete("/api/librarian/tags/{id}", async (int id, [FromServices] ISender sender) =>
            {
                await sender.Send(new DeleteTagCommand(id));
                return Results.NoContent();
            }).RequireAuthorization("Librarian").WithTags("Librarian - Tags");

        }
    }
}
