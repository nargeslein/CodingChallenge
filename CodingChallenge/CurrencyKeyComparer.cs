using System.Collections.Generic;

namespace CodingChallenge
{
    public class CurrencyKeyComparer : IEqualityComparer<CurrencyKey>
    {
        public bool Equals(CurrencyKey x, CurrencyKey y)
        {
            if (x == null || y == null)
                return false; 
            return x.FromCurrency == y.FromCurrency && x.ToCurrency == y.ToCurrency;
        }

        public int GetHashCode(CurrencyKey x)
        {
            return x.GetHashCode();
        }
    }
}