using REST_project.Controllers.Models;
using System.Text.Json;

namespace REST_project.Controllers.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public ExchangeRateService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = "49509bcb0d873f6f2a525581";
            _baseUrl = "https://v6.exchangerate-api.com/v6";
        }

        public async Task<ExchangeRateResponse?> GetExchangeRatesAsync(string baseCurrency = "USD")
        {
            var url = $"{_baseUrl}/{_apiKey}/latest/{baseCurrency}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<ExchangeRateResponse>(jsonContent, options);
            }
            return null;
            
            
        }

        public async Task<ConversionResponse?> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            var url = $"{_baseUrl}/{_apiKey}/pair/{fromCurrency}/{toCurrency}/{amount}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(jsonContent);
                var root = jsonDoc.RootElement;

                if (root.GetProperty("result").GetString() == "success")
                {
                    return new ConversionResponse
                    {
                        FromCurrency = fromCurrency,
                        ToCurrency = toCurrency,
                        Amount = amount,
                        ConvertedAmount = root.GetProperty("conversion_result").GetDecimal(),
                        ExchangeRate = root.GetProperty("conversion_rate").GetDecimal(),
                        LastUpdated = DateTimeOffset.FromUnixTimeSeconds(root.GetProperty("time_last_update_unix").GetInt64()).DateTime
                    };
                }
            }
            return null;
        }

        public async Task<Dictionary<string, string>?> GetSupportedCurrenciesAsync()
        {
            var url = $"{_baseUrl}/{_apiKey}/codes";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(jsonContent);
                var root = jsonDoc.RootElement;

                if (root.GetProperty("result").GetString() == "success")
                    {
                    var supportedCodes = root.GetProperty("supported_codes");
                    var currencies = new Dictionary<string, string>();
                    
                    foreach (var currency in supportedCodes.EnumerateArray())
                    {
                        var code = currency[0].GetString();
                        var name = currency[1].GetString();
                        if (code != null && name != null)
                        {
                            currencies[code] = name;
                        }
                    }
                    return currencies;
                }
            }
            return null;
        }

        public async Task<decimal?> GetSpecificRateAsync(string fromCurrency, string toCurrency)
        {
            var url = $"{_baseUrl}/{_apiKey}/pair/{fromCurrency}/{toCurrency}";
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(jsonContent);
                var root = jsonDoc.RootElement;

                if (root.GetProperty("result").GetString() == "success")
                {
                    return root.GetProperty("conversion_rate").GetDecimal();
                }
            }
            return null;
        }
    }
}