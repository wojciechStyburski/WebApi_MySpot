using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    #region Arrange
    private readonly Date _now;
    private readonly WeeklyParkingSpot _weeklyParkingSpot;
    public WeeklyParkingSpotTests()
    {
        _now = new Date(new DateTime(2022, 11, 15));
        _weeklyParkingSpot = WeeklyParkingSpot.Create(Guid.NewGuid(), new Week(_now), "P1");
    }
    #endregion

    //[Fact]
    [Theory]
    [InlineData("2022-11-14")]
    [InlineData("2022-11-25")]
    public void given_invalid_date_add_reservation_should_fail(string dateString)
    {
        //Arrange
        var invalidDate = DateTime.Parse(dateString);
        var reservation = new VehicleReservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe", "XYZ1234", new Date(invalidDate), 1);
        
        //Act
        var exception = Record.Exception( () => _weeklyParkingSpot.AddReservation(reservation, _now));

        //Assert
        
        /*
        Assert.NotNull(exception);
        Assert.IsType<InvalidReservationDateException>(exception);
        */

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }

    [Fact]
    public void given_reservation_for_already_reserved_parking_spot_add_reservation_should_fail()
    {
        var reservationDate = _now.AddDays(1);
        var reservation = new VehicleReservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe", "ZXC123", reservationDate, 2);
        _weeklyParkingSpot.AddReservation(reservation, _now);
        var nextReservation = new VehicleReservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe", "ZXC123", reservationDate, 1);

        var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(nextReservation, reservationDate));
        
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotCapacityExceededException>();
    }

    [Fact]
    public void given_reservation_for_not_reserved_parking_spot_add_reservation_should_succeed()
    {
        var reservationDate = _now.AddDays(1);
        var reservation = new VehicleReservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe", "ZXC123", reservationDate, 2);

        _weeklyParkingSpot.AddReservation(reservation, _now);

        _weeklyParkingSpot.Reservations.ShouldHaveSingleItem();
    }
}