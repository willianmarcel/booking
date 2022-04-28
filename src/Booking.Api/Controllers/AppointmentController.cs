using Booking.Domain.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IPublishEndpoint _publisher;
    private readonly IMessageScheduler _publisherScheduler;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(IPublishEndpoint publisher, IMessageScheduler publisherScheduler, ILogger<AppointmentController> logger)
    {
        _publisher = publisher;
        _publisherScheduler = publisherScheduler;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AppointmentCreatedEvent insertedEvent)
    {
        await _publisher.Publish(insertedEvent);
        _logger.LogInformation($"Send appointment inserted: {insertedEvent.ClientId} - {insertedEvent.Name} - {insertedEvent.Doctor} - { insertedEvent.Specialty} - {insertedEvent.AppointmentDate}");

        return Ok();
    }

    [HttpPost("schedule")]
    public async Task<IActionResult> PostSchedule([FromBody] AppointmentCreatedEvent insertedEvent)
    {
        await _publisherScheduler.SchedulePublish(DateTime.UtcNow + TimeSpan.FromSeconds(10), insertedEvent);

        _logger.LogInformation($"Send appointment inserted: {insertedEvent.ClientId} - {insertedEvent.Name} - {insertedEvent.Doctor} - { insertedEvent.Specialty} - {insertedEvent.AppointmentDate}");

        return Ok();
    }
}
