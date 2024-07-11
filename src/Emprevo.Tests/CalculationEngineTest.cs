using AutoFixture;
using AutoFixture.AutoMoq;
using Emprevo.Api.Constants;
using Emprevo.Api.Models;
using Emprevo.Api.Services.Rates;
using FluentAssertions;

namespace Emprevo.Tests
{
    public class CalculationEngineTest
    {
        private readonly Fixture _fixture;

        public CalculationEngineTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customize(new AutoMoqCustomization());
        }

        [Fact]
        public void GetCalculator_WhenEntryAtSixAM_ExitAtThreeThirty_ReturnsEarlyBird()
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod("2024-07-12", "2024-07-12", "06:00", "15:30");
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.EarlybirdRateName);
            result.Data.TotalPrice.Should().Be(13);
        }

        [Fact]
        public void GetCalculator_WhenEntryAtNineAM_ExitAtElevenThirtyPM_ReturnsEarlyBird()
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod("2024-07-12", "2024-07-12", "09:00", "23:30");
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.EarlybirdRateName);
            result.Data.TotalPrice.Should().Be(13);
        }

        [Fact]
        public void GetCalculator_WhenEntryAtSixPM_ExitBeforeSixAM_ReturnsNightRate()
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod("2024-07-12", "2024-07-13", "18:00", "05:30");
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.NightRateName);
            result.Data.TotalPrice.Should().Be(6.5m);
        }

        [Fact]
        public void GetCalculator_WhenEntryAtFridayMidnight_ExitBeforeMidnightSunday_ReturnsWeekendRate()
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod("2024-07-13", "2024-07-14", "00:00", "23:59");
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.WeekendRateName);
            result.Data.TotalPrice.Should().Be(20);
        }

        [Fact]
        public void GetCalculator_WhenEntry_ExitDoNotMatchSpecialConditions_ExitWithinOneHour_ReturnsStandardRate()
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod("2024-07-12", "2024-07-12", "09:01", "10:00");
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data.TotalPrice.Should().Be(5);
        }

        [Fact]
        public void GetCalculator_WhenEntry_ExitDoNotMatchSpecialConditions_ExitWithinTwoHours_ReturnsStandardRate()
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod("2024-07-12", "2024-07-12", "09:01", "11:00");
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data.TotalPrice.Should().Be(10);
        }

        [Fact]
        public void GetCalculator_WhenEntry_ExitDoNotMatchSpecialConditions_ExitWithinThreeHours_ReturnsStandardRate()
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod("2024-07-12", "2024-07-12", "09:01", "12:00");
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data.TotalPrice.Should().Be(15);
        }

        [Fact]
        public void GetCalculator_WhenEntry_ExitDoNotMatchSpecialConditions_ExitWithinFourHours_ReturnsStandardRate()
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod("2024-07-12", "2024-07-12", "09:01", "13:00");
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data.TotalPrice.Should().Be(20);
        }

        [Fact]
        public void GetCalculator_WhenEntry_ExitDoNotMatchSpecialConditions_ExitNextday_ReturnsStandardRate()
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod("2024-07-12", "2024-07-13", "09:01", "09:02");
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data.TotalPrice.Should().Be(40);
        }

        [Theory]
        [InlineData("2024-07-12", "2024-07-12", "06:00", "15:31", 13)]
        [InlineData("2024-07-12", "2024-07-12", "06:00", "23:29", 13)]
        [InlineData("2024-07-12", "2024-07-12", "06:00", "23:30", 13)]
        public void GetCalculator_ShouldApplyEarlybirdRates(string entryDate, string exitDate, string entryTime, string exitTime, decimal value)
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod(entryDate, exitDate, entryTime, exitTime);
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.EarlybirdRateName);
            result.Data.TotalPrice.Should().Be(value);
        }

        [Theory]
        [InlineData("2024-07-12", "2024-07-13", "18:00", "05:59", 6.5)]
        [InlineData("2024-07-12", "2024-07-13", "23:59", "05:59", 6.5)]
        public void GetCalculator_ShouldApplyNightRates(string entryDate, string exitDate, string entryTime, string exitTime, decimal value)
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod(entryDate, exitDate, entryTime, exitTime);
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.NightRateName);
            result.Data.TotalPrice.Should().Be(value);
        }

        [Theory]
        [InlineData("2024-07-13", "2024-07-13", "00:00", "05:59", 10)]
        [InlineData("2024-07-13", "2024-07-14", "23:59", "06:01", 10)]
        [InlineData("2024-07-13", "2024-07-14", "00:00", "05:59", 20)]
        public void GetCalculator_ShouldApplyWeekendRates(string entryDate, string exitDate, string entryTime, string exitTime, decimal value)
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod(entryDate, exitDate, entryTime, exitTime);
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.WeekendRateName);
            result.Data.TotalPrice.Should().Be(value);
        }

        [Theory]
        [InlineData("2024-07-12", "2024-07-12", "05:59", "15:30", 20)]
        [InlineData("2024-07-12", "2024-07-12", "06:00", "15:29", 20)]
        [InlineData("2024-07-12", "2024-07-12", "06:00", "23:31", 20)]
        [InlineData("2024-07-12", "2024-07-13", "06:01", "15:30", 40)]
        [InlineData("2024-07-11", "2024-07-13", "23:59", "05:59", 40)]
        public void GetCalculator_ShouldApplyStandardRates(string entryDate, string exitDate, string entryTime, string exitTime, decimal value)
        {
            // Arrange
            var parkingPeriod = new ParkingPeriod(entryDate, exitDate, entryTime, exitTime);
            var calculationEngine = _fixture.Create<CalculationEngine>();

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data.TotalPrice.Should().Be(value);
        }
    }
}