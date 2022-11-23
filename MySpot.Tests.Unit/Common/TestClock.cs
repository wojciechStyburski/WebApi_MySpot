using MySpot.Application.Services;
using MySpot.Core.Abstractions;

namespace MySpot.Tests.Unit.Common;

public class TestClock : IClock
{
    public DateTime Current() => new DateTime(2022, 11, 20, 9, 0, 0);
}