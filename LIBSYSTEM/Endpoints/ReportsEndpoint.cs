using Application.Queries;
using MediatR;

namespace LIBSYSTEM.Endpoints
{
    public static class ReportsEndpoint
    {
        public static void MapReportsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/reports/most-borrowed", async (int count, ISender sender) =>
            {
                var result = await sender.Send(new GetMostBorrowedBooksQuery { Count = count });
                return Results.Ok(result);
            }).RequireAuthorization("Librarian");

            app.MapGet("/api/reports/least-borrowed", async (int count, ISender sender) =>
            {
                var result = await sender.Send(new GetLeastBorrowedBooksQuery { Count = count });
                return Results.Ok(result);
            }).RequireAuthorization("Librarian");

        }
    }
}
