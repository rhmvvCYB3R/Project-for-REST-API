using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REST_project.Controllers.Models;
using REST_project.Controllers.Services;

namespace REST_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all exchange rate endpoints
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        /// <summary>
        /// Get all exchange rates for a base currency
        /// </summary>
        /// <param name="baseCurrency">Base currency code (default: USD)</param>
        [HttpGet("rates")]
        public async Task<IActionResult> GetExchangeRates([FromQuery] string baseCurrency = "USD")
        {
            var rates = await _exchangeRateService.GetExchangeRatesAsync(baseCurrency);

            if (rates == null)
                return BadRequest(new { message = "Failed to fetch exchange rates." });

            return Ok(rates);
        }

        /// <summary>
        /// Convert currency amount
        /// </summary>
        /// <param name="request">Conversion request with FromCurrency, ToCurrency, and Amount</param>
        [HttpPost("convert")]
        public async Task<IActionResult> ConvertCurrency([FromBody] ConversionRequest request)
        {
            if (string.IsNullOrEmpty(request.FromCurrency) || string.IsNullOrEmpty(request.ToCurrency))
                return BadRequest(new { message = "FromCurrency and ToCurrency are required." });

            if (request.Amount <= 0)
                return BadRequest(new { message = "Amount must be greater than 0." });

            var conversion = await _exchangeRateService.ConvertCurrencyAsync(
                request.FromCurrency,
                request.ToCurrency,
                request.Amount);

            if (conversion == null)
                return BadRequest(new { message = "Failed to convert currency. Please check currency codes." });

            return Ok(conversion);
        }

        /// <summary>
        /// Get specific exchange rate between two currencies
        /// </summary>
        /// <param name="fromCurrency">Source currency code</param>
        /// <param name="toCurrency">Target currency code</param>
        [HttpGet("rate/{fromCurrency}/{toCurrency}")]
        public async Task<IActionResult> GetSpecificRate(string fromCurrency, string toCurrency)
        {
            var rate = await _exchangeRateService.GetSpecificRateAsync(fromCurrency, toCurrency);

            if (rate == null)
                return BadRequest(new { message = "Failed to fetch exchange rate. Please check currency codes." });

            return Ok(new
            {
                fromCurrency = fromCurrency,
                toCurrency = toCurrency,
                exchangeRate = rate,
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Get list of supported currencies
        /// </summary>
        [HttpGet("currencies")]
        public async Task<IActionResult> GetSupportedCurrencies()
        {
            var currencies = await _exchangeRateService.GetSupportedCurrenciesAsync();

            if (currencies == null)
                return BadRequest(new { message = "Failed to fetch supported currencies." });

            return Ok(new { supportedCurrencies = currencies });
        }

        /// <summary>
        /// Get popular currency pairs rates
        /// </summary>
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularRates()
        {
            var popularPairs = new[]
            {
                new { From = "USD", To = "EUR" },
                new { From = "USD", To = "GBP" },
                new { From = "USD", To = "JPY" },
                new { From = "EUR", To = "USD" },
                new { From = "GBP", To = "USD" }
            };

            var rates = new List<object>();

            foreach (var pair in popularPairs)
            {
                var rate = await _exchangeRateService.GetSpecificRateAsync(pair.From, pair.To);
                if (rate.HasValue)
                {
                    rates.Add(new
                    {
                        fromCurrency = pair.From,
                        toCurrency = pair.To,
                        exchangeRate = rate.Value
                    });
                }
            }

            return Ok(new { popularRates = rates, timestamp = DateTime.UtcNow });
        }
    }
}