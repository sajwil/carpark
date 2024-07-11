using Emprevo.Api.Constants;
using Emprevo.Api.Models;
using Emprevo.Api.Services.Rates;
using Microsoft.AspNetCore.Mvc;

namespace Emprevo.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CalculationController(ICalculationEngineService calculationEngine, ILogger<CalculationController> logger) : ControllerBase
    {
        private readonly ICalculationEngineService _calculationEngine = calculationEngine;
        private readonly ILogger<CalculationController> _logger = logger;

        [HttpPost]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CalculateRate([FromBody] ParkingPeriod parkingPeriod)
        {
            try
            {
                var result = _calculationEngine.CalculateRate(parkingPeriod);

                if (result.Status != ResultCode.Ok)
                {
                    return BadRequest(result.Message);
                }

                return Ok(result.Data);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "CalculateRate error.");
                return BadRequest(ex.Message);
            }
        }
    }
}