using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public sealed class CleaningReservation : Reservation
{
    public CleaningReservation(ReservationId id, Date date) : base(id, capacity: 2, date)
    {
    }
}