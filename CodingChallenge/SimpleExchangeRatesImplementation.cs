using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{
    /// <summary>
    /// simple implementation of exchange rates using a dictionary 
    /// </summary>
    public class SimpleExchangeRatesImplementation : IExchangeRates
    {
        private readonly Dictionary<CurrencyKey, decimal> _rates = new Dictionary<CurrencyKey, decimal>(new CurrencyKeyComparer());

        public decimal GetRate(string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency)
                return 1; 

            CurrencyKey key = new CurrencyKey(fromCurrency, toCurrency);
            if (_rates.ContainsKey(key))
                return _rates[key];
            throw new Exception($"Currency pair {key.FromCurrency} {key.ToCurrency} is not contained");
        }

        public void AddRate(string fromCurrency, string toCurrency, decimal rate)
        {
            AddRate(new CurrencyKey(fromCurrency, toCurrency), rate);
        }

        public void AddRate(CurrencyKey key, decimal rate)
        {
            if (!_rates.ContainsKey(key))
                _rates.Add(key, rate);
        }
    }
}

