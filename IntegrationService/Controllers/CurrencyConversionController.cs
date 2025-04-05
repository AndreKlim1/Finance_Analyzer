using IntegrationService.DTO;
using IntegrationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Controllers
{
    [ApiController]
    [Route("api/v1.0/conversion")]
    public class ConversionController : ControllerBase
    {
        private readonly ICurrencyConversionService _conversionService;

        public ConversionController(ICurrencyConversionService conversionService)
        {
            _conversionService = conversionService;
        }


        [HttpGet("rates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRates(CancellationToken cancellationToken)
        {
            try
            {
                var rates = await _conversionService.GetLatestRatesAsync(cancellationToken);
                return Ok(rates);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        
        [HttpPost("convert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConvertCurrency([FromBody] CurrencyConversionRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest("Request body is null");

            try
            {
                var convertedValue = await _conversionService.ConvertCurrencyAsync(request.TargetCurrency, request.TransactionCurrency, request.TransactionValue, cancellationToken);
                return Ok(new CurrencyConversionResponse { ConvertedValue = convertedValue });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
