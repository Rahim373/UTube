using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Prometheus;

namespace UTube.Common.Extensions
{
    public static class ApplicationExtensions
    {
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }

        public static IEndpointRouteBuilder MapPrometheus(this IEndpointRouteBuilder routeBuilder, IApplicationBuilder app)
        {
            app.UseHttpMetrics(options =>
            {
                options.AddCustomLabel("host", context => context.Request.Host.Host);
            });

            routeBuilder.MapHealthChecks("/health");
            routeBuilder.MapMetrics();

            return routeBuilder;
        }
    }
}
