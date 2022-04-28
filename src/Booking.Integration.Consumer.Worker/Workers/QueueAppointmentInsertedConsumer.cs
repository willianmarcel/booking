using Booking.Domain.Events;
using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MassTransit.Metadata;
using System.Diagnostics;

namespace Booking.Integration.Consumer.Worker.Workers;

public class QueueAppointmentInsertedConsumer : IConsumer<AppointmentCreatedEvent>
{
    private readonly ILogger<QueueAppointmentInsertedConsumer> _logger;

    public QueueAppointmentInsertedConsumer(ILogger<QueueAppointmentInsertedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AppointmentCreatedEvent> context)
    {
        var timer = Stopwatch.StartNew();

        try
        {
            var id = context.Message.ClientId;
            var name = context.Message.Name;
            var email = context.Message.Email;

            await context.Publish(new SendEmailEvent { Email = email });

            _logger.LogInformation($"Receive client: {id} - {name}");
            await context.NotifyConsumed(timer.Elapsed, TypeMetadataCache<QueueAppointmentInsertedConsumer>.ShortName);
        }
        catch (Exception ex)
        {
            await context.NotifyFaulted(timer.Elapsed, TypeMetadataCache<QueueAppointmentInsertedConsumer>.ShortName, ex);
        }
    }
}

public class QueueAppointmentConsumerDefinition : ConsumerDefinition<QueueAppointmentInsertedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueAppointmentInsertedConsumer> consumerConfigurator)
    {
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}
