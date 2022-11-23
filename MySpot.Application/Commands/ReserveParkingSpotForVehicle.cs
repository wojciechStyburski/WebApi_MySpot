using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands;

public sealed record ReserveParkingSpotForVehicle(Guid ParkingSpotId, Guid ReservationId, Guid UserId, DateTime Date,
    string LicensePlate, int Capacity) : ICommand;
