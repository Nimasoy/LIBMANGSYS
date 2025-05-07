using LIBSYSTEM.Endpoints;

namespace LIBSYSTEM
{
    public static class EndpointRegistration
    {
        public static void MapAllEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapBookEndpoints();
            app.MapUsersEndpoints();
            app.MapCategoriesEndpoints();
            app.MapTagsEndpoints();
            app.MapReportsEndpoints();
            app.MapRegisterEndpoints();
        }
    }
}
