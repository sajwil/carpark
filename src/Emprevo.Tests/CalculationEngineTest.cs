//using AutoFixture;
//using AutoFixture.AutoMoq;
//using Emprevo.Api.Services;
//using FluentAssertions;

//namespace Emprevo.Tests
//{
//    public class CalculationEngineTest
//    {
//        private readonly Fixture _fixture;

//        public CalculationEngineTest()
//        {
//            _fixture = new Fixture();
//            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
//                .ForEach(b => _fixture.Behaviors.Remove(b));
//            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
//            _fixture.Customize(new AutoMoqCustomization());
//        }

//        [Fact]
//        public void CalculateRate_WhenEnterAtSixAmAndExitThreeThirty_ReturnsThirteen()
//        {
//            // Arrange
//            var entryDateTime = new DateTime(2024, 7, 11, 6, 0, 0, DateTimeKind.Utc);
//            var exitDateTime = new DateTime(2024, 7, 11, 15, 30, 0, DateTimeKind.Utc);

//            var calculationEnginer = _fixture.Create<CalculationEngine>();

//            // Act
//            var result = calculationEnginer.CalculateRate(entryDateTime, exitDateTime);

//            // Assert
//            result.Data.Should().Be(13.00m);
//        }
//    }
//}