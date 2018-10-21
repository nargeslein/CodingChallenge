namespace CodingChallenge
{/// <summary>
/// custom key for currency pair
/// </summary>
    public class CurrencyKey
    {
        public readonly string FromCurrency;
        public readonly string ToCurrency;

        public CurrencyKey(string c1, string c2)
        {
            FromCurrency = c1;
            ToCurrency = c2;

        }

        public override int GetHashCode()
        {

            return FromCurrency.GetHashCode() ^ ToCurrency.GetHashCode();
        }
    }
}