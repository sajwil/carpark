using AutoFixture;
using AutoFixture.AutoMoq;

namespace Emprevo.Tests
{
    public class RateCalculationStrategyTest
    {
        private readonly Fixture _fixture;

        public RateCalculationStrategyTest()
        {

            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customize(new AutoMoqCustomization());
        }
    }
}