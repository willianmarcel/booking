using Booking.Api.Core;
using Booking.Integration.Consumer.Worker.Workers;
using MassTransit;
using MassTransit.Definition;
using Serilog;

SerilogExtensions.AddSerilog("Booking Integration Consumer Worker");

var appSettings = new AppSettings();

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog(Log.Logger)
    .ConfigureServices((context, collection) =>
    {
        context.Configuration.Bind(appSettings);
        collection.AddOpenTelemetry(appSettings);
        collection.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();
            x.AddConsumer<QueueAppointmentInsertedConsumer>(typeof(QueueAppointmentConsumerDefinition));
            x.AddConsumer<QueueSendEmailConsumer>(typeof(QueueSendEmailConsumerDefinition));

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(context.Configuration.GetConnectionString("RabbitMq"));
                cfg.UseDelayedMessageScheduler();
                cfg.ServiceInstance(instance =>
                {
                    instance.ConfigureJobServiceEndpoints();
                    instance.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));
                });
            });
        });
        collection.AddMassTransitHostedService(true);
    })
    .Build();

await host.StartAsync();

Console.WriteLine("Waiting for new messages.");

while (true);
