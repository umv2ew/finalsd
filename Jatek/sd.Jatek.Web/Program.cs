using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using sd.Jatek.Application.Models;
using sd.Jatek.Infrastructure;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();


builder.Services.AddDbContext<GameContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddHealthChecks()
    .AddRabbitMQ(new Uri("amqp://guest:guest@rabbitmq:5672"))
    .AddDbContextCheck<GameContext>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq();
});

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "_myAllowSpecificOrigins",
//                      policy =>
//                      {
//                          policy.WithOrigins("http://example.com");
//                          policy.WithMethods("GET", "POST");
//                          policy.AllowCredentials();
//                      });
//});

var assemblies = Assembly.Load("sd.Jatek.Application");

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Game}/{action=StartGame}/{id?}");

app.MapHub<GameHub>("/GameHub");

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

    var context = services.GetRequiredService<GameContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();
