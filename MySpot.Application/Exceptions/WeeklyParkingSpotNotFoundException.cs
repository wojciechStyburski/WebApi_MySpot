﻿using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public class WeeklyParkingSpotNotFoundException : CustomException
{
    public Guid Id { get; }
    public WeeklyParkingSpotNotFoundException()
        : base("Weekly parking spot with ID was not found.")
    {
    }
    public WeeklyParkingSpotNotFoundException(Guid id) 
        : base($"Weekly parking spot with id {id} was not found")
    {
        Id = id;
    }
}