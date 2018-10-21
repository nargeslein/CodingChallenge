using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodingChallenge
{
    /// <summary>
    /// asset portfolio class
    /// </summary>
    public class AssetPortfolio
    {
        /// <summary>
        /// Domestic currency 
        /// </summary>
        public const string EUR = "EUR";

        public String Name { get; set; }

        public AssetPortfolio(IExchangeRates exchangeRates, string name)
        {
            ExchangeRates = exchangeRates;
            Name = name;
        }

        public AssetPortfolio(IExchangeRates exchangeRates, string name, List<Asset> assets)
        {
            ExchangeRates = exchangeRates;
            Name = name;
            assets.ForEach(Add);
        }


        /// <summary>
        /// list of assets in portfolio
        /// </summary>
        private List<Asset> Portfolio { get; } = new List<Asset>();

        private readonly Dictionary<Asset, List<Asset>> _symbols = new Dictionary<Asset, List<Asset>>();

        public int NoDistinctAssets => _symbols.Keys.Count;

        public int NoAssets => Portfolio.Count;

        /// <summary>
        /// Exchange rates used to value the portfolio
        /// </summary>
        public IExchangeRates ExchangeRates { get; set; }

        public void Add(Asset s)
        {
            if (_symbols.ContainsKey(s))
                _symbols[s].Add(s);
            else
                _symbols.Add(s, new List<Asset> { s });
            Portfolio.Add(s);
        }

        public decimal Value(string toCurrency)
        {
            decimal v = 0;
            foreach (var s in Portfolio)
                v = v + s.Value(ExchangeRates, toCurrency);
            return v;
        }

        public decimal Value()
        {
            return Value(EUR); ;
        }

        public AssetPortfolio Consolidate()
        {
            AssetPortfolio ap = new AssetPortfolio(ExchangeRates, $"consolidated  {Name}");
            foreach (KeyValuePair<Asset, List<Asset>> asset in _symbols)
            {
                Asset newAsset = asset.Key.Consolidate(asset.Value);
                ap.Add(newAsset);
            }
            return ap;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Portfolio '{Name}' value EUR {Value():0,0.00} with {NoAssets} assets:");
            foreach (var asset in Portfolio)
                sb.AppendLine("\t" + asset);
            return sb.ToString();
        }
    }
}