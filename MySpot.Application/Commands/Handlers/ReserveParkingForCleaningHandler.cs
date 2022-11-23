using MySpot.Application.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public class ReserveParkingForCleaningHandler : ICommandHandler<ReserveParkingForCleaning>
{
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
    private readonly IParkingReservationService _reservationService;

    public ReserveParkingForCleaningHandler(IWeeklyParkingSpotRepository weeklyParkingSpotRepository, IParkingReservationService reservationService)
    {
        _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
        _reservationService = reservationService;
    }

    public async Task HandleAsync(ReserveParkingForCleaning command)
    {
        var week = new Week(command.Date);
        var weeklyParkingSpots = (await _weeklyParkingSpotRepository.GetByWeekAsync(week)).ToList();

        _reservationService.ReserveParkingForCleaning(weeklyParkingSpots, new Date(command.Date));

        var tasks = weeklyParkingSpots.Select(x => _weeklyParkingSpotRepository.UpdateAsync(x));
        await Task.WhenAll(tasks);
    }
}