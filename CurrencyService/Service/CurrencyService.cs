using CurrencyService.Extensions;
using CurrencyService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace CurrencyService
{
    public class CurrencyService : ICurrencyService
    {
        private readonly string _serviceUrl;
        public bool IsDisposed { get; private set; } = false;

        private List<CurrencyInfoModel> currencyInfoModels;

        public CurrencyService(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
            ConnectLoadData();
        }

        private void ConnectLoadData()
        {
            if (string.IsNullOrEmpty(_serviceUrl))
                throw new Exception("Invalid service url");
            try
            {
                XDocument xDoc = XDocument.Load(_serviceUrl);

                currencyInfoModels = xDoc.Descendants("Currency").Select(x => new CurrencyInfoModel
                {
                    TurkishCurrencyName = x.Element("Isim").Value,
                    Unit = x.Element("Unit").Value.ConvertTo<int>(),
                    CurrencyCode = x.Attribute("CurrencyCode").Value,
                    CurrencyName = x.Element("CurrencyName").Value,
                    ForexBuying = x.Element("ForexBuying").Value.ConvertTo<decimal?>(),
                    ForexSelling = x.Element("ForexSelling").Value.ConvertTo<decimal?>(),
                    BanknoteBuying = x.Element("BanknoteBuying").Value.ConvertTo<decimal?>(),
                    BanknoteSelling = x.Element("BanknoteSelling").Value.ConvertTo<decimal?>(),
                    CrossRateUSD = x.Element("CrossRateUSD").Value.ConvertTo<decimal?>(),
                    CrossRateOther = x.Element("CrossRateOther").Value.ConvertTo<decimal?>()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot connect to service..", ex);
            }
        }

        public IEnumerable<CurrencyInfoModel> GetCurrencies()
        {
            return currencyInfoModels;
        }

        public IEnumerable<CurrencyInfoModel> GetCurrencies<TOrderBy>(Expression<Func<CurrencyInfoModel, bool>> filter, Expression<Func<CurrencyInfoModel, TOrderBy>> orderBy)
        {

            return currencyInfoModels.AsQueryable().Where(filter).OrderBy(orderBy).ToList();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;
            GC.SuppressFinalize(this);
        }


    }
}
