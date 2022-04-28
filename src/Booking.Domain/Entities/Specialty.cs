﻿namespace Booking.Domain.Entities;

public class Specialty : Entity
{
    public Specialty(string? code, string? description)
    {
        Code = code;
        Description = description;
    }

    public string? Code { get; private set; }
    public string? Description { get; private set; }
}