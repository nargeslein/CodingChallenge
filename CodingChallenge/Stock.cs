using System;
using System.Collections.Generic;

namespace CodingChallenge
{
    /// <summary>
    /// implementation of stock with share and price, this is general stock
    /// </summary>
    public class Stock : Asset
    {
        /// <summary>
        /// Shares as property
        /// </summary>
        public decimal Shares { get; set; }
        /// <summary>
        /// Price as property
        /// </summary>
        public decimal Price { get; set; }

        public Stock(string symbol, string currency, decimal shares, decimal price) : base(symbol, currency)
        {
            Shares = shares;
            Price = price;
        }
        /// <summary>
        /// The value of a stock is shares * price
        /// </summary>
        /// <returns></returns>
        public override decimal Value()
        {
            return Shares * Price;
        }
        /// <summary>
        /// consolidates a list of Stocks that are equal, having the same symbol and currency and same asset type,
        /// note that domestic stock and stock in EUR cannot be consolidated even if the symbols are the same as the type is different
        /// </summary>
        /// <param name="assets"></param>
        /// <returns></returns>
        public override Asset Consolidate(List<Asset> assets)
        {
            ConsolidateCheck(assets);
            decimal newShares = 0;
            decimal newPrice = 0;

            foreach (var stock in assets)
            {
                Stock s = (Stock)stock;
                newShares += s.Shares;
                newPrice += (s.Price * s.Shares);
            }
            newPrice = newShares != 0 ? newPrice / newShares : 0;
            return new Stock(Symbol, Currency, newShares, newPrice);
        }
    }
}