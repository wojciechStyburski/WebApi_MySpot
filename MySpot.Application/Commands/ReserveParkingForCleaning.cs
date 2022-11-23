using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands;

public record ReserveParkingForCleaning(DateTime Date) : ICommand;
