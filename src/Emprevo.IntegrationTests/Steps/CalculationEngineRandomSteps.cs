using Emprevo.IntegrationTests.Models;
using Gherkin.Ast;
using System.Globalization;
using Xunit.Gherkin.Quick;

namespace Emprevo.IntegrationTests.Steps
{
    [FeatureFile("./Features/CalculationEngineRandom.feature")]
    public class CalculationEngineRandomSteps : TestBase
    {
        private DateTime? entryDateTime;
        private DateTime? exitDateTime;
        private CalculationResult? result;

        [Given(@"a customer parks their car:")]
        public async Task WhenACustomerParksTheirCarAsync(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows.Skip(1))
            {
                // Arrange
                entryDateTime = DateTime.Parse(row.Cells.ElementAt(0).Value, CultureInfo.InvariantCulture);
                exitDateTime = DateTime.Parse(row.Cells.ElementAt(1).Value, CultureInfo.InvariantCulture);
                result = null;

                // Act
                result = await GetDataFromApi<CalculationResult>(new Dictionary<string, DateTime>
                {
                    { "entryDateTime", entryDateTime.Value },
                    { "exitDateTime", exitDateTime.Value }
                });

                // Assert
                Assert.Equal(decimal.Parse(row.Cells.ElementAt(2).Value), result.TotalPrice);
            }
        }
    }
}