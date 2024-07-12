using Emprevo.IntegrationTests.Models;
using System.Globalization;
using Xunit.Gherkin.Quick;

namespace Emprevo.IntegrationTests.Steps
{
    [FeatureFile("./Features/CalculationEngine.feature")]
    public sealed class CalculationEngineTest : TestBase
    {
        private DateTime? entryDateTime;
        private DateTime? exitDateTime;
        private CalculationResult? result;

        [When(@"a customer parks their car")]
        public void WhenACustomerParksTheirCar()
        {
            entryDateTime = null;
            exitDateTime = null;
            result = null;
        }

        [And(@"the customer enters the parking lot at ""(.*)""")]
        public void WhenTheCustomerEntersTheParkingLotAt(string entry)
        {
            entryDateTime = DateTime.Parse(entry, CultureInfo.InvariantCulture);
        }

        [And(@"the customer exits the parking lot at ""(.*)""")]
        public void WhenTheCustomerExitsTheParkingLotAt(string exit)
        {
            exitDateTime = DateTime.Parse(exit, CultureInfo.InvariantCulture);
        }

        [Then(@"the total parking price should be \$(.*)")]
        public async Task ThenTheTotalParkingPriceShouldBeAsync(decimal expectedPrice)
        {
            result = await GetDataFromApi<CalculationResult>(new Dictionary<string, DateTime>
            {
                { "entryDateTime", entryDateTime.Value },
                { "exitDateTime", exitDateTime.Value }
            });

            Assert.Equal(expectedPrice, result.TotalPrice);
        }

        [And(@"the rate applied should be ""(.*)""")]
        public void ThenTheRateAppliedShouldBe(string expectedRate)
        {
            Assert.Equal(expectedRate, result.RateName);
        }
    }
}