using Emprevo.Api.Models;
using FluentAssertions;

namespace Emprevo.Tests
{
    public class ParkingPeriodTest
    {
        [Fact]
        public void ParkingPeriod_WhenEntryDateIsEmpty_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ParkingPeriod("", "2024-07-12", "18:30", "19:30"));
            exception.Message.Should().Be("Invalid entry date format");
        }

        [Fact]
        public void ParkingPeriod_WhenExitDateIsInvalid_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ParkingPeriod("2024-07-11", "2024-13-12", "18:30", "19:30"));
            exception.Message.Should().Be("Invalid exit date format");
        }

        [Fact]
        public void ParkingPeriod_WhenEntryTimeIsInvalid_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ParkingPeriod("2024-07-11", "2024-07-12", "18:30:00", "19:30"));
            exception.Message.Should().Be("Invalid entry time format");
        }

        [Fact]
        public void ParkingPeriod_WhenExitTimeIsInvalid_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ParkingPeriod("2024-07-11", "2024-07-12", "18:30", "19:30:00"));
            exception.Message.Should().Be("Invalid exit time format");
        }

        [Fact]
        public void ParkingPeriod_WhenEntryDateTimeIsAfterExitDateTime_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ParkingPeriod("2024-07-12", "2024-07-11", "18:30", "19:30"));
            exception.Message.Should().Be("Entry date and time must be before exit date and time");
        }

        [Fact]
        public void ParkingPeriod_WhenValidArgumentsProvided_CreatesParkingPeriod()
        {
            // Arrange
            var entryDate = "2024-07-12";
            var exitDate = "2024-07-12";
            var entryTime = "18:30";
            var exitTime = "19:30";

            // Act
            var parkingPeriod = new ParkingPeriod(entryDate, exitDate, entryTime, exitTime);

            // Assert
            parkingPeriod.EntryDateTime.Should().Be(new DateTime(2024, 7, 12, 18, 30, 0));
            parkingPeriod.ExitDateTime.Should().Be(new DateTime(2024, 7, 12, 19, 30, 0));
        }
    }
}
