using AutoFixture;
using AutoFixture.AutoMoq;
using Emprevo.Api.Constants;
using Emprevo.Api.Extensions;
using Emprevo.Api.Models;
using Emprevo.Api.Services.Rates;
using Emprevo.Api.Services.Rates.Calculators;
using FluentAssertions;
using Moq;

namespace Emprevo.Tests
{
    public class CalculationEngineTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IServiceProvider> _mockServiceProvider;

        public CalculationEngineTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customize(new AutoMoqCustomization());

            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockServiceProvider
                .Setup(x => x.GetService(typeof(EarlybirdRateCalculator)))
                .Returns(_fixture.Create<EarlybirdRateCalculator>());
            _mockServiceProvider
                .Setup(x => x.GetService(typeof(NightRateCalculator)))
                .Returns(_fixture.Create<NightRateCalculator>());
            _mockServiceProvider
                .Setup(x => x.GetService(typeof(WeekendRateCalculator)))
                .Returns(_fixture.Create<WeekendRateCalculator>());
            _mockServiceProvider
                .Setup(x => x.GetService(typeof(StandardRateCalculator)))
                .Returns(_fixture.Create<StandardRateCalculator>());
        }

        [Fact]
        public void GetCalculator_WhenEntryAtSixAM_ExitAtThreeThirty_ReturnsEarlyBird()
        {
            // Arrange
            var entryDateTime = new DateTime(2024, 7, 12, 6, 0, 0, DateTimeKind.Utc);
            var exitDateTime = new DateTime(2024, 7, 12, 15, 30, 0, DateTimeKind.Utc);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.EarlybirdRateName);
            result.Data?.TotalPrice.Should().Be(13);
        }

        [Fact]
        public void GetCalculator_WhenEntryAtNineAM_ExitAtElevenThirtyPM_ReturnsEarlyBird()
        {
            // Arrange
            var entryDateTime = new DateTime(2024, 7, 12, 9, 0, 0, DateTimeKind.Utc);
            var exitDateTime = new DateTime(2024, 7, 12, 23, 30, 0, DateTimeKind.Utc);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.EarlybirdRateName);
            result.Data?.TotalPrice.Should().Be(13);
        }

        [Fact]
        public void GetCalculator_WhenEntryAtSixPM_ExitBeforeSixAM_ReturnsNightRate()
        {
            // Arrange
            var entryDateTime = new DateTime(2024, 7, 12, 18, 0, 0, DateTimeKind.Utc);
            var exitDateTime = new DateTime(2024, 7, 13, 5, 30, 0, DateTimeKind.Utc);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.NightRateName);
            result.Data?.TotalPrice.Should().Be(6.5m);
        }

        [Fact]
        public void GetCalculator_WhenEntryAtFridayMidnight_ExitBeforeMidnightSunday_ReturnsWeekendRate()
        {
            // Arrange
            var entryDateTime = new DateTime(2024, 7, 13, 0, 0, 0, DateTimeKind.Utc);
            var exitDateTime = new DateTime(2024, 7, 14, 23, 59, 59, DateTimeKind.Utc);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.WeekendRateName);
            result.Data?.TotalPrice.Should().Be(20);
        }

        [Fact]
        public void GetCalculator_WhenEntry_ExitDoNotMatchSpecialConditions_ExitWithinOneHour_ReturnsStandardRate()
        {
            // Arrange
            var entryDateTime = new DateTime(2024, 7, 12, 9, 1, 0, DateTimeKind.Utc);
            var exitDateTime = new DateTime(2024, 7, 12, 10, 0, 0, DateTimeKind.Utc);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data?.TotalPrice.Should().Be(5);
        }

        [Fact]
        public void GetCalculator_WhenEntry_ExitDoNotMatchSpecialConditions_ExitWithinTwoHours_ReturnsStandardRate()
        {
            // Arrange
            var entryDateTime = new DateTime(2024, 7, 12, 9, 1, 0, DateTimeKind.Utc);
            var exitDateTime = new DateTime(2024, 7, 12, 11, 0, 0, DateTimeKind.Utc);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data?.TotalPrice.Should().Be(10);
        }

        [Fact]
        public void GetCalculator_WhenEntry_ExitDoNotMatchSpecialConditions_ExitWithinThreeHours_ReturnsStandardRate()
        {
            // Arrange
            var entryDateTime = new DateTime(2024, 7, 12, 9, 1, 0, DateTimeKind.Utc);
            var exitDateTime = new DateTime(2024, 7, 12, 12, 0, 0, DateTimeKind.Utc);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data?.TotalPrice.Should().Be(15);
        }

        [Fact]
        public void GetCalculator_WhenEntry_ExitDoNotMatchSpecialConditions_ExitWithinFourHours_ReturnsStandardRate()
        {
            // Arrange
            var entryDateTime = new DateTime(2024, 7, 12, 9, 1, 0, DateTimeKind.Utc);
            var exitDateTime = new DateTime(2024, 7, 12, 13, 0, 0, DateTimeKind.Utc);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data?.TotalPrice.Should().Be(20);
        }

        [Fact]
        public void GetCalculator_WhenEntry_ExitDoNotMatchSpecialConditions_ExitNextday_ReturnsStandardRate()
        {
            // Arrange
            var entryDateTime = new DateTime(2024, 7, 12, 9, 1, 0, DateTimeKind.Utc);
            var exitDateTime = new DateTime(2024, 7, 13, 9, 2, 0, DateTimeKind.Utc);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data?.TotalPrice.Should().Be(25);
        }

        [Theory]
        [InlineData("2024-07-12T06:00", "2024-07-12T15:31", 13)]
        [InlineData("2024-07-12T06:00", "2024-07-12T23:29", 13)]
        [InlineData("2024-07-12T06:00", "2024-07-12T23:30", 13)]
        public void GetCalculator_ShouldApplyEarlybirdRates(string entryDateTimeStr, string exitDateTimeStr, decimal value)
        {
            // Arrange
            var entryDateTime = entryDateTimeStr.ToDate();
            var exitDateTime = exitDateTimeStr.ToDate();
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.EarlybirdRateName);
            result.Data?.TotalPrice.Should().Be(value);
        }

        [Theory]
        [InlineData("2024-07-12T18:00", "2024-07-13T05:59", 6.5)]
        [InlineData("2024-07-12T23:59", "2024-07-13T05:59", 6.5)]
        [InlineData("2024-07-11T18:12", "2024-07-11T23:12", 6.5)]
        public void GetCalculator_ShouldApplyNightRates(string entryDateTimeStr, string exitDateTimeStr, decimal value)
        {
            // Arrange
            var entryDateTime = entryDateTimeStr.ToDate();
            var exitDateTime = exitDateTimeStr.ToDate();
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.NightRateName);
            result.Data?.TotalPrice.Should().Be(value);
        }

        [Theory]
        [InlineData("2024-07-13T00:00", "2024-07-13T05:59", 10)]
        [InlineData("2024-07-13T23:59", "2024-07-14T06:01", 10)]
        [InlineData("2024-07-13T00:00", "2024-07-14T05:59", 20)]
        public void GetCalculator_ShouldApplyWeekendRates(string entryDateTimeStr, string exitDateTimeStr, decimal value)
        {
            // Arrange
            var entryDateTime = entryDateTimeStr.ToDate();
            var exitDateTime = exitDateTimeStr.ToDate();
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.WeekendRateName);
            result.Data?.TotalPrice.Should().Be(value);
        }

        [Theory]
        [InlineData("2024-07-12T05:59", "2024-07-12T15:30", 20)]
        [InlineData("2024-07-12T06:00", "2024-07-12T15:29", 20)]
        [InlineData("2024-07-12T06:00", "2024-07-12T23:31", 20)]
        [InlineData("2024-07-12T06:01", "2024-07-13T15:30", 40)]
        [InlineData("2024-07-11T23:59", "2024-07-13T05:59", 40)]
        [InlineData("2024-07-11T18:00", "2024-07-12T18:01", 25)]
        public void GetCalculator_ShouldApplyStandardRates(string entryDateTimeStr, string exitDateTimeStr, decimal value)
        {
            // Arrange
            var entryDateTime = entryDateTimeStr.ToDate();
            var exitDateTime = exitDateTimeStr.ToDate();
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data?.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data?.TotalPrice.Should().Be(value);
        }
    }
}