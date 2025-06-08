using REST_project.Controllers.Models;

namespace REST_project.Controllers.Services
{
    public interface IExchangeRateService
    {
        Task<ExchangeRateResponse?> GetExchangeRatesAsync(string baseCurrency = "USD");
        Task<ConversionResponse?> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount);
        Task<Dictionary<string, string>?> GetSupportedCurrenciesAsync();
        Task<decimal?> GetSpecificRateAsync(string fromCurrency, string toCurrency);
    }
}