using Application.Queries.Books;
using MediatR;

namespace LIBSYSTEM.Endpoints
{
    public static class ReportsEndpoint
    {
        public static void MapReportsEndpoints(this IEndpointRouteBuilder app)
        {
            // librarian-only access

            // view most borrowed books report
            app.MapGet("/api/reports/librarian/most-borrowed", async (int count, ISender sender) =>
            {
                var result = await sender.Send(new GetMostBorrowedBooksQuery { Count = count });
                return Results.Ok(result);
            }).RequireAuthorization("Librarian").WithTags("Librarian - Reports");
            // view least borrowed books report
            app.MapGet("/api/reports/librarian/least-borrowed", async (int count, ISender sender) =>
            {
                var result = await sender.Send(new GetLeastBorrowedBooksQuery { Count = count });
                return Results.Ok(result);
            }).RequireAuthorization("Librarian").WithTags("Librarian - Reports");
        }
    }
}
