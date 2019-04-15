using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
namespace Dashboard
{
    class DataSource
    {

        public static ConcurrentQueue<Object> RTUpdatesQueue = new ConcurrentQueue<Object>();
        
        public static ConcurrentDictionary<string,byte> WatchedTickers = new ConcurrentDictionary<string, byte>();//a way to get a concurrent set (no duplicates)

        public void StartRTWatch()
        {
            // watch OMS
            Action watch_oms = () =>
            {
                // some how pull oms system and enqueue into OMSUpdates
                int mult = 1;
                while (true)
                {
                    Position updated_pos = new  Position()
                    {
                        Underlying = new Security()
                        {
                            Name = "BNP",
                            Ask = 0.0,
                            Bid = 0.0,
                            Sector = "Europe"
                        },

                        BuyAveragePrice = 23.9 + mult * 0.3,
                        BuyQuantity = 80077 + mult * 30,
                        SellAveragePrice = 23.8 + mult * 0.21,
                        SellQuantity = 690008 + mult * 37,
                    };
                    RTUpdatesQueue.Enqueue(updated_pos);
                    if ( mult % 5 == 0)
                    {
                        Position new_pos = new Position()
                        {
                            Underlying = new Security()
                            {
                                Name = "NEW_" + Convert.ToString(mult),
                                Ask = 0.0,
                                Bid = 0.0,
                                Sector = "Europe"
                            },
                            PortfolioName = "European0",
                            BuyAveragePrice = 153.9 + mult * 0.06,
                            BuyQuantity = 80077 + mult * 30,
                            SellAveragePrice = 23.8 + mult * 0.21,
                            SellQuantity = 690008 + mult * 37,
                        };

                        RTUpdatesQueue.Enqueue(new_pos);
                        WatchedTickers.TryAdd(new_pos.Underlying.Name, 0);
                    }
                    System.Threading.Thread.Sleep(9600);
                    mult++;
                }
            };

            // watch bloom
            Action watch_bloomberg = () =>
            {
                // some how pull oms system and enqueue into MarketDataUpdates
                int mult = 1;
                while (true)
                {
                    Security update_ticker = new Security()
                    {
                        Name = "BNP",
                        Ask = 4.6 + mult*0.23,
                        Bid = 4.6 + mult * 0.23
                    };
                    RTUpdatesQueue.Enqueue(update_ticker);
                    foreach(var pair in WatchedTickers)
                    {
                        Security update_ticker_new = new Security()
                        {
                            Name = pair.Key,
                            Ask = 5.6 + mult * 0.23,
                            Bid = 3.7 + mult * 0.23
                        };
                        RTUpdatesQueue.Enqueue(update_ticker_new);
                    }
                    System.Threading.Thread.Sleep(1500);
                    mult++;
                }

            };
            Task.Factory.StartNew( () =>
            {
                Parallel.Invoke(watch_oms, watch_bloomberg);
            });
        }
    }
}
