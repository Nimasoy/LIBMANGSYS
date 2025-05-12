using Application.Commands.Login;
using MediatR;

namespace LIBSYSTEM.Endpoints
{
    public static class RegisterEndpoint
    {
        public static void MapRegisterEndpoints(this IEndpointRouteBuilder app)
        {
            // all users access

            // login
            app.MapPost("/api/users/login", async (CreateLoginCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            }).WithTags("Login");

        }
    }
}
