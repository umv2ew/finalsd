using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using sd.Statisztika.Application.Consumers;
using sd.Statisztika.Infrastructure;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StatisticsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddHealthChecks()
    .AddRabbitMQ(new Uri("amqp://guest:guest@rabbitmq:5672"))
    .AddDbContextCheck<StatisticsContext>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StatisticsConsumer>();

    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
});

var assemblies = Assembly.Load("sd.Statisztika.Application");

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));


var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    ResultStatusCodes = new Dictionary<HealthStatus, int>
    {
        {HealthStatus.Healthy, StatusCodes.Status200OK},
        {HealthStatus.Degraded, StatusCodes.Status503ServiceUnavailable},
        {HealthStatus.Unhealthy, StatusCodes.Status503ServiceUnavailable},
    },
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<StatisticsContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();