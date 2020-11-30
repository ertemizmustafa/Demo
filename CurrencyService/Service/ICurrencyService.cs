using CurrencyService.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CurrencyService
{
    public interface ICurrencyService : IDisposable
    {
        IEnumerable<CurrencyInfoModel> GetCurrencies();
        IEnumerable<CurrencyInfoModel> GetCurrencies<TOrderBy>(Expression<Func<CurrencyInfoModel, bool>> filter, Expression<Func<CurrencyInfoModel, TOrderBy>> orderBy);

        bool IsDisposed { get; }

    }
}
