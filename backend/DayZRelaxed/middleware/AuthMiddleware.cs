using DayZRelaxed.Helpers;

namespace DayZRelaxed.middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            if (!context.Request.Cookies.ContainsKey("token")) {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Bad Request");
                return;
            }
            var jwt = context.Request.Cookies["token"];
            var jwtHelper = new JWTHelper() { Token = jwt };

            if (jwtHelper.ValidateJWT()) await _next(context);
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }
        }
    }

    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder AuthMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}