using Carter;
using VideoService.Api;
using VideoService.Application;
using VideoService.Application.GRpc;
using VideoService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApplication(builder.Configuration)
        .AddInfrastructure(builder.Configuration)
        .AddPresentation(builder.Configuration);
};

var app = builder.Build();
{
    app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
    app.MapGrpcService<GrpcVideoService>();
    app.MapGrpcReflectionService();
    app.MapCarter();
    app.MapPrometheusScrapingEndpoint();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(conf =>
    {
        conf.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });

    app.Run();
};
