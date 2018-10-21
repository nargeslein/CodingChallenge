using System;
using System.Collections.Generic;
using CodingChallenge;
using NUnit.Framework;


namespace CodingChallengeTesting
{
    [TestFixture]
    public class Tests
    {
        private SimpleExchangeRatesImplementation _rates;
        private SimpleExchangeRatesImplementation _flatrates;
        const string EUR = "EUR";
        const string GBP = "GBP";
        const string USD = "USD";

        private List<Asset> _t1;
        private List<Asset> _t2;
        private List<Asset> _t3;
        private List<Asset> _t4;


        [Test]
        public void TestExchangeRateDuplicate()
        {
            _rates.AddRate(EUR, EUR, 1);
            Assert.Pass();
        }

        [SetUp]
        public void TestRatesSetup()
        {
            _flatrates = new SimpleExchangeRatesImplementation();
            _rates = new SimpleExchangeRatesImplementation();
            //Add flat rates
            string[] c = { EUR, USD, GBP };

            foreach (var c1 in c)
            {
                _rates.AddRate(c1, c1, 1);
                foreach (var c2 in c)
                    _flatrates.AddRate(c1, c2, 1);
            }

            //Add other rates
            _rates.AddRate(EUR, GBP, 2m);
            _rates.AddRate(EUR, USD, 0.5m);
            _rates.AddRate(GBP, EUR, 0.5m);
            _rates.AddRate(USD, EUR, 2m);
            _rates.AddRate(GBP, USD, 0.25m);
            _rates.AddRate(USD, GBP, 4m);
        }

        public void TestAssetSetup()
        {
            _t1 = new List<Asset>
            {
                new DomesticStock("ABC", 200, 4),
                new DomesticStock("ABC", 200, 4)
            };

            _t2 = new List<Asset>
            {
                new Stock("ABCGBP", GBP, 200, 4),
                new DomesticStock("ABC", 200, 4)
            };

            _t3 = new List<Asset>
            {
                new Stock("ABCGBP", GBP, 200, 4),
                new Stock("ABCUSD", USD, 200, 4),
                new DomesticStock("ABC", 200, 4)
            };

            _t4 = new List<Asset>
            {
                new Stock("ABCGBP", GBP, 200, 4),
                new Stock("ABCUSD", USD, 200, 4),
                new DomesticStock("ABC", 200, 4),
                new FXCurrencyAsset("GBP", GBP, 100),
                new FXCurrencyAsset("USD", USD, 100)
            };
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {


        }

        public IEnumerable<TestCaseData> GetCases
        {
            get
            {
                TestAssetSetup();
                TestRatesSetup();
                yield return new TestCaseData(_t1, _flatrates, "Domestic stock portfolio", 1600m, EUR);
                yield return new TestCaseData(_t2, _flatrates, "Simple stock portfolio", 1600m, EUR);
                yield return new TestCaseData(_t3, _flatrates, "Stock Portfolio", 2400m, EUR);
                yield return new TestCaseData(_t4, _flatrates, "Mixed Portfolio", 2600m, EUR);
                yield return new TestCaseData(_t1, _flatrates, "Domestic stock portfolio", 1600m, USD);
                yield return new TestCaseData(_t2, _flatrates, "Simple stock portfolio", 1600m, GBP);
                yield return new TestCaseData(_t3, _flatrates, "Stock Portfolio", 2400m, USD);
                yield return new TestCaseData(_t4, _flatrates, "Mixed Portfolio", 2600m, GBP);

                yield return new TestCaseData(_t1, _rates, "Domestic stock portfolio", 1600m, EUR);
                yield return new TestCaseData(_t1, _rates, "Domestic stock portfolio", 800m, USD);
                yield return new TestCaseData(_t2, _rates, "Simple stock portfolio", 1200m, EUR);
                yield return new TestCaseData(_t3, _rates, "Stock Portfolio", 2800m, EUR);
                yield return new TestCaseData(_t3, _rates, "Stock Portfolio", 5600m, GBP);
                yield return new TestCaseData(_t3, _rates, "Stock Portfolio", 1400m, USD);

            }
        }

        [Test]
        [TestCaseSource(nameof(GetCases))]
        public void TestValuationPortfolio(List<Asset> assets, IExchangeRates rates, string name, decimal expected, string currency)
        {
            AssetPortfolio portfolio = new AssetPortfolio(rates, name, assets);
            TestValuationPortfolio(portfolio, expected, currency);
            Console.WriteLine(portfolio.ToString());
        }

        public void TestValuationPortfolio(AssetPortfolio portfolio, decimal expected, string currency)
        {
            decimal actual = portfolio.Value(currency);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TestConsolidationPortfolio()
        {

            List<Asset> assets = new List<Asset>
                {
                        new Stock("ABCGBP", "GBP", 200, 4),
                        new Stock("ABCGBP", "GBP", 200, 4),

                        new Stock("ABCUSD", "USD", 200, 4),

                        new DomesticStock("ABC", 200, 4),
                        new DomesticStock("ABC", 100, 8),
                        new DomesticStock("ABC", 400, 8),

                        new FXCurrencyAsset("GBP", "GBP", 100),
                        new FXCurrencyAsset("GBP", "GBP", 200),
                        new FXCurrencyAsset("GBP", "GBP", 300),

                        new FXCurrencyAsset("USD", "USD", 100),
                        new FXCurrencyAsset("USD", "USD", 200),
                        new FXCurrencyAsset("USD", "USD", 300)
                        };

            var portfolio = new AssetPortfolio(_flatrates, "Original Portfolio", assets);


            TestValuationPortfolio(portfolio, 8400, EUR);

            //there are 12 assets
            Assert.AreEqual(portfolio.NoAssets, 12);

            //there are 5 distinct assets
            Assert.AreEqual(portfolio.NoDistinctAssets, 5);

            //Consolidating Portfolio
            AssetPortfolio portfolioConsolidated = portfolio.Consolidate();

            TestValuationPortfolio(portfolioConsolidated, 8400, EUR);

            //consolidated into 5 assets
            Assert.AreEqual(portfolioConsolidated.NoDistinctAssets, 5);
            Assert.AreEqual(portfolioConsolidated.NoAssets, 5);

            Console.WriteLine(portfolio);
            Console.WriteLine(portfolioConsolidated);

        }


        [Test]
        public void TestAddAssetsSameSymbolAndType()
        {
            Assert.DoesNotThrow(delegate
              {
                  var portfolio = new AssetPortfolio(_flatrates, "Test Asset Portfolio");
                  portfolio.Add(new FXCurrencyAsset("ABC", EUR, 20));
                  portfolio.Add(new FXCurrencyAsset("ABC", EUR, 10));
                  var consolidated = portfolio.Consolidate(); 

                  Assert.AreEqual(portfolio.NoAssets,2);
                  Assert.AreEqual(portfolio.NoDistinctAssets, 1);
                  Assert.AreEqual(consolidated.NoAssets,consolidated.NoDistinctAssets,1);
              });

        }

        [Test]
        public void TestAddAssetsSameSymbol()
        {
            Assert.DoesNotThrow(delegate
            {
                var portfolio = new AssetPortfolio(_flatrates, "Test Asset Portfolio");
                portfolio.Add(new FXCurrencyAsset("ABC", EUR, 20));
                portfolio.Add(new Stock("ABC", EUR, 10, 20));
                portfolio.Add(new DomesticStock("ABC", 10, 20));
                var consolidated = portfolio.Consolidate();

                Assert.AreEqual(portfolio.NoAssets,portfolio.NoDistinctAssets, 3);
                Assert.AreEqual(consolidated.NoAssets, consolidated.NoDistinctAssets, 3);

            });
        }

        [Test]
        public void TestConsolidationEmptyList()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(delegate
            {
                var list = new List<Asset>(); 
                Asset asset = new Stock("ABCGBP", GBP, 200, 4);
                asset.Consolidate(list);
            });
            Assert.That(ex.Message, Is.EqualTo("empty asset list"));
        }


        [Test]
        public void TestConsolidationDifferentAssets()
        {
            ConsolidationException ex = Assert.Throws<ConsolidationException>(delegate
            {
                var list = new List<Asset>
                {

                    new Stock("ABCUSD", USD, 200, 4),
                    new DomesticStock("ABC", 200, 4),
                    new FXCurrencyAsset("GBP", GBP, 100),
                    new FXCurrencyAsset("USD", USD, 100)
                };
                Asset asset = new Stock("ABCGBP", GBP, 200, 4);
                asset.Consolidate(list);
            });
        }

        [Test]
        public void TestConsolidationSameAssets()
        {
            Assert.DoesNotThrow(delegate
            {
                var list = new List<Asset>
                {
                    new FXCurrencyAsset(GBP, GBP, 100),
                    new FXCurrencyAsset(GBP, GBP, 100),
                    new FXCurrencyAsset(GBP, GBP, 100),
                    new FXCurrencyAsset(GBP, GBP, 100),
                };
                Asset asset = new FXCurrencyAsset(GBP, GBP, 100);

                Asset newAsset = asset.Consolidate(list);
                Asset newExpectedAsset = new FXCurrencyAsset(GBP, GBP, 400);
                Assert.True(newExpectedAsset.Equals(newAsset));
            });

        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {

        }
    }
}

