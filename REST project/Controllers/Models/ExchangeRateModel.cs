namespace REST_project.Controllers.Models
{
    public class ExchangeRateResponse
    {
        public string Result { get; set; }
        public string Documentation { get; set; }
        public string Terms_of_use { get; set; }
        public long Time_last_update_unix { get; set; }
        public string Time_last_update_utc { get; set; }
        public long Time_next_update_unix { get; set; }
        public string Time_next_update_utc { get; set; }
        public string Base_code { get; set; }
        public Dictionary<string, decimal> Conversion_rates { get; set; }
    }

    public class ConversionRequest
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
    }

    public class ConversionResponse
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class SupportedCurrenciesResponse
    {
        public string Result { get; set; }
        public Dictionary<string, string> Supported_codes { get; set; }
    }
}