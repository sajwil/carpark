using Emprevo.Api.Constants;
using Emprevo.Api.Models;
using Emprevo.Api.Services.Rates;
using Microsoft.AspNetCore.Mvc;

namespace Emprevo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationEngine _calculationEngine;

        public CalculationController(ICalculationEngine calculationEngine)
        {
            _calculationEngine = calculationEngine;
        }

        // change this
        [HttpPost]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public IActionResult GetRate(string entryDate, string exitDate, string entryTime, string exitTime)
        {
            var result = _calculationEngine.CalculateRate(new ParkingPeriod(entryDate, exitDate, entryTime, exitTime));

            if (result.Status == ResultCode.Ok)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}