using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{   /// <summary>
    /// abstract class for Asset which can be extended for any kind of asset
    /// </summary>
    public abstract class Asset
    {
        /// <summary>
        /// Asset symbol as property
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Asset currency as property
        /// </summary>
        public string Currency { get; }

        /// <summary>
        /// Abstract method returning the value of the asset
        /// </summary>
        /// <returns></returns>
        public abstract decimal Value();

        protected Asset(string symbol, string currency)
        {
            Symbol = symbol;
            Currency = currency;
        }

        /// <summary>
        /// every derived class must provide its own implementation how assets are consolidated
        /// </summary>
        /// <param name="assets"></param>
        /// <returns></returns>
        public abstract Asset Consolidate(List<Asset> assets);
        
        /// <summary>
        /// method to check if assets are equal
        /// </summary>
        /// <param name="assets"></param>
        protected void ConsolidateCheck(List<Asset> assets)
        {
            if (assets.Count == 0 || assets==null)
                throw new ArgumentException("empty asset list");

            foreach (var asset in assets)
            {
                if (!Equals(asset))
                    throw new ConsolidationException(
                        $"Assets are different, asset {asset} cannot be consolidated with asset {this}");
            }
        }

        /// <summary>
        /// method to value asset in another currency
        /// </summary>
        /// <param name="exchangeRates"></param>
        /// <param name="toCurrency"></param>
        /// <returns></returns>
        public decimal Value(IExchangeRates exchangeRates, string toCurrency)
        {
            return Value() * exchangeRates.GetRate(Currency, toCurrency);
        }

        public override string ToString()
        {
            return $"Asset '{Symbol}' is {GetType().Name} - value {Currency} {Value():0,0.00}";
        }
        /// <summary>
        /// Overriding Equals method so that assets from the same type and same currency and symbol are considered equal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Asset asset = obj as Asset;
            if (asset == null)
                return false;
            return Currency == asset.Currency && Symbol == asset.Symbol && GetType() == asset.GetType();
        }

        public override int GetHashCode() => Currency.GetHashCode() ^ Symbol.GetHashCode() ^ GetType().GetHashCode();
    }
}
