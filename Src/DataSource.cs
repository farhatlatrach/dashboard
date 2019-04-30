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

        public static ConcurrentQueue<Security> WatchedTickers = new ConcurrentQueue<Security>();

        public static void StartRTWatch()
        {
            // watch OMS
           Action watch_oms = () =>
           {
             //Mahmoud add OMS start here
             /*
              start the callback which should enqueue new/updated position into "RTUpdatesQueue"
              
               */
           };

        //    // watch bloom this action is starting at the start of the program
        Action watch_bloomberg = () =>
        {
        log4net.ILog Log =
           log4net.LogManager.GetLogger("DataSource.StartWatch.watch_bloomberg");// System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
            /*
             * //Mahmoud 
             * all the securities for bloom to deal with for both RT ans static data 
             * are going to be in the "WatchedTickers" queue as a Security object
             * (The push in the queue will be done from the portfolio form side (once there a is a new security 
             * coming from OMS in a new position))
             * So I suggest you do the following:
             * 1- start an empty subscription to bloomberg
             * 2- whithin a while(true) keep waiting anything coming in the queue
             * 3- once you have a security in the queue 1) get its static data and push in the RT updates then 2) add it to the subscription for RT.
             */
            // some how pull oms system and enqueue into MarketDataUpdates
            int mult = 1;
            while (true)
            {
                Log.Info("waiting prices..");
                PriceUpdate update_ticker = new PriceUpdate("AFX GY", 4.6 + mult * 0.23);
              
                RTUpdatesQueue.Enqueue(update_ticker);
               
                System.Threading.Thread.Sleep(1500);
                while(false == WatchedTickers.IsEmpty)
                {
                    Security sec = new Security("");
                    WatchedTickers.TryDequeue(out sec);
                   // Security sec2 = new Security(sec.Name);
                    sec.Currency = "EUR";
                    sec.Country = "France";
                    sec.QuotationFactor = 1;
                    sec.Sector = "Sector 1";
                    RTUpdatesQueue.Enqueue(sec);
                }
                PriceUpdate update_ticker_fx = new PriceUpdate("EUR/USD", 0.84 + mult * 0.23);
                RTUpdatesQueue.Enqueue(update_ticker_fx);
                mult++;
            }

        };
        Task.Factory.StartNew(() =>
            {
                Parallel.Invoke(watch_bloomberg,watch_oms);
            });
        }
    }
}
