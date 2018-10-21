using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{   /// <inheritdoc />
    /// <summary>
    /// implementation of FX currency asset with amount in FX currency
    /// </summary>
    public class FXCurrencyAsset : Asset
    {
        /// <summary>
        /// Amount of FX currency 
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// constructor for creating FX currency asset
        /// </summary>
        /// <param name="symbol">asset symbol</param>
        /// <param name="currency">currency of the asset</param>
        /// <param name="amount">amount of asset in FX currency</param>
        public FXCurrencyAsset(string symbol, string currency, decimal amount) : base(symbol, currency)
        {
            Amount = amount;
        }

        /// <summary>
        /// The value of a FX currency is the amount in FX currency
        /// </summary>
        /// <returns></returns>
        public override decimal Value()
        {
            return Amount;
        }

        /// <summary>
        /// consolidates a list of asset from the same asset class, similarity is based on the asset symbol
        /// </summary>
        /// <param name="assets">list of assets</param>
        /// <returns>consolidated asset</returns>
        protected override Asset CustomConsolidate(List<Asset> assets)
        {
            decimal amount = 0;
            foreach (var fx in assets)
                amount += ((FXCurrencyAsset)fx).Amount;
            return new FXCurrencyAsset(Symbol, Currency, amount);
        }
    }
}
