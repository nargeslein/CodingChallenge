using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace CodingChallenge
{
    /// <summary>
    /// interface for exchange rates
    /// </summary>
    public interface IExchangeRates
    {
        decimal GetRate(string fromCurrency, string toCurrency);

    }
}