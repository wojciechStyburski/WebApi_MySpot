using MySpot.Core.Abstractions;

namespace MySpot.Tests.Unit.Common;

public class TestClock : IClock
{
    public DateTime Current() => new DateTime(2022, 10, 20, 9, 0, 0);
}