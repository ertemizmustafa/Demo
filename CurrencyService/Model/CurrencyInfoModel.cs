namespace CurrencyService.Model
{
    public class CurrencyInfoModel
    {
        public string TurkishCurrencyName { get; set; }
        public int Unit { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public decimal? ForexBuying { get; set; }
        public decimal? ForexSelling { get; set; }
        public decimal? BanknoteBuying { get; set; }
        public decimal? BanknoteSelling { get; set; }
        public decimal? CrossRateUSD { get; set; }
        public decimal? CrossRateOther { get; set; }

    }
}
