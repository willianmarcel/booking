namespace Booking.Domain.Events;

public class AppointmentCreatedEvent
{
    public string? ClientId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Specialty { get; set; }
    public string? Doctor { get; set; }
    public DateTime AppointmentDate { get; set; }
}
