using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
namespace Dashboard
{
    public class DataSource
    {

        public static ConcurrentQueue<Object> RTUpdatesQueue = new ConcurrentQueue<Object>();
        
        public static ConcurrentDictionary<string,byte> WatchedTickers = new ConcurrentDictionary<string, byte>();//a way to get a concurrent set (no duplicates)

        public static void StartRTWatch()
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
                            PreviousClose = 0.0,
                            Last = 0.0,
                            Sector = "Europe"
                        },

                        BoughtAveragePrice = 23.9 + mult * 0.3,
                        BoughtQuantity = 80077 + mult * 30,
                        SoldAveragePrice = 23.8 + mult * 0.21,
                        SoldQuantity = 690008 + mult * 37,
                        BeginOfDayQuantity = 2700
                    };
                    RTUpdatesQueue.Enqueue(updated_pos);
                    if ( mult % 5 == 0)
                    {
                        Position new_pos = new Position()
                        {
                            Underlying = new Security()
                            {
                                Name = "NEW_" + Convert.ToString(mult),
                                PreviousClose = 0.0,
                                Last = 0.0,
                                Sector = "Europe"
                            },
                            PortfolioName = "European0",
                            BoughtAveragePrice = 153.9 + mult * 0.06,
                            BoughtQuantity = 80077 + mult * 30,
                            SoldAveragePrice = 23.8 + mult * 0.21,
                            SoldQuantity = 690008 + mult * 37,
                            BeginOfDayQuantity = 3400
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
                        PreviousClose = 4.6,
                        Last = 4.6 + mult * 0.23
                    };
                    RTUpdatesQueue.Enqueue(update_ticker);
                    foreach(var pair in WatchedTickers)
                    {
                        Security update_ticker_new = new Security()
                        {
                            Name = pair.Key,
                            PreviousClose = 5.6,
                            Last = 3.7 + mult * 0.23
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
