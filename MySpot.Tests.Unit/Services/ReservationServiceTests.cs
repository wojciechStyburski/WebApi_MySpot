using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Policies;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DAL.Repositories;
using MySpot.Tests.Unit.Common;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    #region Arrange

    private readonly IClock _clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
    private readonly IReservationsService _reservationsService;

    public ReservationServiceTests()
    {
        _clock = new TestClock();
        _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);

        var parkingReservationService = new ParkingReservationService(new IReservationPolicy[]
        {
            new BossReservationPolicy(),
            new ManagerReservationPolicy(),
            new RegularEmployeeReservationPolicy(_clock)
        }, 
        _clock);

        _reservationsService = new ReservationsService(_clock, _weeklyParkingSpotRepository, parkingReservationService);
    }

    #endregion


    [Fact]
    public async Task given_reservation_for_not_taken_date_create_reservation_should_succeed()
    {
        //Arrange
        var weeklyParkingSpot = (await _weeklyParkingSpotRepository.GetAllAsync()).First();
        var command = new ReserveParkingSpotForVehicle(weeklyParkingSpot.Id, Guid.NewGuid(), DateTime.UtcNow.AddMinutes(30), "John Doe", "XYZ123", 1);

        //Act
        var reservationId = await _reservationsService.ReserveForVehicleAsync(command);

        //Assert
        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }
}