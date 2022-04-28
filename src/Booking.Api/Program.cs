using Booking.Api.Core;
using Serilog;

SerilogExtensions.AddSerilog("Booking Api");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Log.Logger);

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddOpenTelemetry(appSettings);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransitExtension(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
