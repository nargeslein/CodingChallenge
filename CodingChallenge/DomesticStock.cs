using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{/// <inheritdoc />
    /// <summary>
    /// implementation of domestic stock as subclass of Stock
    /// </summary>
	public class DomesticStock : Stock
	{
		public DomesticStock(string symbol, decimal shares, decimal price) : base(symbol, AssetPortfolio.EUR, shares, price)
		{

		}
	}
}