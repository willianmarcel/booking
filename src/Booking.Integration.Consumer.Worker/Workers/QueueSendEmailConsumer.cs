﻿using Booking.Domain.Events;
using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace Booking.Integration.Consumer.Worker.Workers;

public class QueueSendEmailConsumer : IConsumer<SendEmailEvent>
{
    private readonly ILogger<QueueSendEmailConsumer> _logger;

    public QueueSendEmailConsumer(ILogger<QueueSendEmailConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendEmailEvent> context)
    {
        _logger.LogInformation($"Email successfully sent: {context.Message.Email}");
    }
}

public class QueueSendEmailConsumerDefinition : ConsumerDefinition<QueueSendEmailConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueSendEmailConsumer> consumerConfigurator)
    {
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}
