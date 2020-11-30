using CurrencyService.Extensions;
using System.Linq;
using Xunit;

namespace CurrencyService.Test
{
    public class CurrencyServiceTest
    {
        private const string serviceUrl = "https://www.tcmb.gov.tr/kurlar/today.xml";

        [Fact]
        public void GetCurrencies()
        {
            using var currenyService = new CurrencyService(serviceUrl);
            var result = currenyService.GetCurrencies();
            Assert.NotNull(result);
        }

        [Fact]
        public void FilterCurrencies()
        {
            using var currenyService = new CurrencyService(serviceUrl);
            var result = currenyService.GetCurrencies();
            var totalCount = result.Count();

            var filterResult = currenyService.GetCurrencies(x => x.BanknoteBuying > 0 && x.CurrencyCode.Contains("U"), x => x.CurrencyName);

            Assert.True(totalCount > filterResult.Count());
        }

        [Fact]
        public void GenerateAndSaveExcelFile()
        {
            using var currenyService = new CurrencyService(serviceUrl);
            var fileResult = currenyService.GetCurrencies().GenerateExcelFile().SaveFile();

            Assert.True(fileResult.IsSuccessfull);
        }

        [Fact]
        public void GenerateAndSaveCsvFile()
        {
            using var currenyService = new CurrencyService(serviceUrl);
            var fileResult = currenyService.GetCurrencies().GenerateCsvFile().SaveFile();

            Assert.True(fileResult.IsSuccessfull);
        }

        [Fact]
        public void GenerateAndSaveJsonFile()
        {
            using var currenyService = new CurrencyService(serviceUrl);
            var fileResult = currenyService.GetCurrencies().GenerateJsonFile().SaveFile();

            Assert.True(fileResult.IsSuccessfull);
        }

    }
}
