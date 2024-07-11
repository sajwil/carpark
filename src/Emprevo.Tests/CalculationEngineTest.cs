using AutoFixture;
using AutoFixture.AutoMoq;
using Emprevo.Api.Constants;
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
            var entryDateTime = new DateTime(2024, 7, 12, 6, 0, 0);
            var exitDateTime = new DateTime(2024, 7, 12, 15, 30, 0);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

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
            var entryDateTime = new DateTime(2024, 7, 12, 9, 0, 0);
            var exitDateTime = new DateTime(2024, 7, 12, 23, 30, 0);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

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
            var entryDateTime = new DateTime(2024, 7, 12, 18, 0, 0);
            var exitDateTime = new DateTime(2024, 7, 13, 5, 30, 0);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

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
            var entryDateTime = new DateTime(2024, 7, 13, 0, 0, 0);
            var exitDateTime = new DateTime(2024, 7, 14, 23, 59, 59);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

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
            var entryDateTime = new DateTime(2024, 7, 12, 9, 1, 0);
            var exitDateTime = new DateTime(2024, 7, 12, 10, 0, 0);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

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
            var entryDateTime = new DateTime(2024, 7, 12, 9, 1, 0);
            var exitDateTime = new DateTime(2024, 7, 12, 11, 0, 0);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

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
            var entryDateTime = new DateTime(2024, 7, 12, 9, 1, 0);
            var exitDateTime = new DateTime(2024, 7, 12, 12, 0, 0);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

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
            var entryDateTime = new DateTime(2024, 7, 12, 9, 1, 0);
            var exitDateTime = new DateTime(2024, 7, 12, 13, 0, 0);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

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
            var entryDateTime = new DateTime(2024, 7, 12, 9, 1, 0);
            var exitDateTime = new DateTime(2024, 7, 13, 9, 2, 0);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.StandardRateName);
            result.Data.TotalPrice.Should().Be(40);
        }

        [Theory]
        [InlineData("2024-07-12T06:00", "2024-07-12T15:31", 13)]
        [InlineData("2024-07-12T06:00", "2024-07-12T23:29", 13)]
        [InlineData("2024-07-12T06:00", "2024-07-12T23:30", 13)]
        public void GetCalculator_ShouldApplyEarlybirdRates(string entryDateTimeStr, string exitDateTimeStr, decimal value)
        {
            // Arrange
            var entryDateTime = DateTime.Parse(entryDateTimeStr);
            var exitDateTime = DateTime.Parse(exitDateTimeStr);
            var parkingPeriod = new ParkingPeriod(entryDateTime, exitDateTime);
            var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

            // Act
            var result = calculationEngine.CalculateRate(parkingPeriod);

            // Assert
            result.Data.RateName.Should().Be(RateNameConstants.EarlybirdRateName);
            result.Data.TotalPrice.Should().Be(value);
        }

        //[Theory]
        //[InlineData("2024-07-12", "2024-07-13", "18:00", "05:59", 6.5)]
        //[InlineData("2024-07-12", "2024-07-13", "23:59", "05:59", 6.5)]
        //public void GetCalculator_ShouldApplyNightRates(string entryDate, string exitDate, string entryTime, string exitTime, decimal value)
        //{
        //    // Arrange
        //    var parkingPeriod = new ParkingPeriod(entryDate, exitDate, entryTime, exitTime);
        //    var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

        //    // Act
        //    var result = calculationEngine.CalculateRate(parkingPeriod);

        //    // Assert
        //    result.Data.RateName.Should().Be(RateNameConstants.NightRateName);
        //    result.Data.TotalPrice.Should().Be(value);
        //}

        //[Theory]
        //[InlineData("2024-07-13", "2024-07-13", "00:00", "05:59", 10)]
        //[InlineData("2024-07-13", "2024-07-14", "23:59", "06:01", 10)]
        //[InlineData("2024-07-13", "2024-07-14", "00:00", "05:59", 20)]
        //public void GetCalculator_ShouldApplyWeekendRates(string entryDate, string exitDate, string entryTime, string exitTime, decimal value)
        //{
        //    // Arrange
        //    var parkingPeriod = new ParkingPeriod(entryDate, exitDate, entryTime, exitTime);
        //    var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

        //    // Act
        //    var result = calculationEngine.CalculateRate(parkingPeriod);

        //    // Assert
        //    result.Data.RateName.Should().Be(RateNameConstants.WeekendRateName);
        //    result.Data.TotalPrice.Should().Be(value);
        //}

        //[Theory]
        //[InlineData("2024-07-12", "2024-07-12", "05:59", "15:30", 20)]
        //[InlineData("2024-07-12", "2024-07-12", "06:00", "15:29", 20)]
        //[InlineData("2024-07-12", "2024-07-12", "06:00", "23:31", 20)]
        //[InlineData("2024-07-12", "2024-07-13", "06:01", "15:30", 40)]
        //[InlineData("2024-07-11", "2024-07-13", "23:59", "05:59", 40)]
        //public void GetCalculator_ShouldApplyStandardRates(string entryDate, string exitDate, string entryTime, string exitTime, decimal value)
        //{
        //    // Arrange
        //    var parkingPeriod = new ParkingPeriod(entryDate, exitDate, entryTime, exitTime);
        //    var calculationEngine = new CalculationEngineService(_mockServiceProvider.Object);

        //    // Act
        //    var result = calculationEngine.CalculateRate(parkingPeriod);

        //    // Assert
        //    result.Data.RateName.Should().Be(RateNameConstants.StandardRateName);
        //    result.Data.TotalPrice.Should().Be(value);
        //}
    }
}