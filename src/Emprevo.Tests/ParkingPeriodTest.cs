using Emprevo.Api.Models;
using FluentAssertions;

namespace Emprevo.Tests
{
    public class ParkingPeriodTest
    {
        [Fact]
        public void ParkingPeriod_WhenEntryDateTimeIsAfterExitDateTime_ThrowsArgumentException()
        {
            var entryDateTime = new DateTime(2024, 9, 12, 9, 1, 0);
            var exitDateTime = new DateTime(2024, 8, 13, 9, 2, 0);
            var exception = Assert.Throws<ArgumentException>(() => new ParkingPeriod(entryDateTime, exitDateTime));
            exception.Message.Should().Be("Entry date and time must be before exit date and time");
        }

        [Fact]
        public void ParkingPeriod_WhenValidArgumentsProvided_CreatesParkingPeriod()
        {
            // Arrange
            var entryDateTime = new DateTime(2024, 7, 12, 9, 1, 0);
            var exitDateTime = new DateTime(2024, 7, 13, 9, 2, 0);

            // Act
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);

            // Assert
            parkingPeriod.EntryDateTime.Should().Be(new DateTime(2024, 7, 12, 9, 1, 0));
            parkingPeriod.ExitDateTime.Should().Be(new DateTime(2024, 7, 13, 9, 2, 0));
        }
    }
}
