using System;
using System.Threading.Tasks;
using YahooFinanceApi;
using System.Linq;
using System.Collections.Generic;

namespace MarketData
{
    class Program
    {
        static async Task Main(string[] args)
        {
            char continueStr = 'y';
            while(continueStr == 'y')
            {
                Console.WriteLine("Enter a stock ticker that you want historic data for...");
                string symbol = Console.ReadLine().ToUpper();
                Console.WriteLine("Enter the amount of months of historic data...");
                int months = Convert.ToInt32(Console.ReadLine());
                DateTime endDate = DateTime.Today;
                DateTime startDate = DateTime.Today.AddMonths(-months);
                var stockData = new StoockData();
                var awaiter = stockData.getStockData(symbol, startDate, endDate);
                if (awaiter.Result == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Do you want to get another ticker's historic data ? (y/n)");
                    continueStr = Convert.ToChar(Console.ReadLine());
                }
                Console.WriteLine("Thank you");
            }
        }

        class StoockData
        {
            public async Task<int> getStockData(string symbol, DateTime startDate, DateTime endDate)
            {
                try
                {
                    var list_to_return = new List<decimal>();
                    var historicData = await Yahoo.GetHistoricalAsync(symbol, startDate, endDate);
                    var security = await Yahoo.Symbols(symbol).Fields(Field.LongName).QueryAsync();
                    var ticker = security[symbol];
                    var companyName = ticker[Field.LongName];
                    for (int i=0; i < historicData.Count; i++)
                    {
                        Console.WriteLine(companyName + " Closing price on: " + historicData.ElementAt(i).DateTime.Day + "/" + historicData.ElementAt(i).DateTime.Year + ": $" + Math.Round(historicData.ElementAt(i).Close, 2));
                        list_to_return.Add(Math.Round(historicData.ElementAt(i).Close));
                    }

                }
                catch
                {
                    Console.WriteLine("Failed to get symbol :" + symbol);
                }
                return 1;
            }
        }
    }
}