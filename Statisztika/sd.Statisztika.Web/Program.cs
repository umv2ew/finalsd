using MassTransit;
using Microsoft.EntityFrameworkCore;
using sd.Statisztika.Application.Consumers;
using sd.Statisztika.Infrastructure;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<StatisticsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

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

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var context = services.GetRequiredService<StatisticsContext>();
//    if (context.Database.GetPendingMigrations().Any())
//    {
//        context.Database.Migrate();
//    }
//}

app.Run();