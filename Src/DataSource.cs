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
                 * 
                 *  if (false == DataSource.RTUpdatesQueue.IsEmpty)
                    {
                        Object obj = new Object();
                        if (DataSource.RTUpdatesQueue.TryDequeue(out obj))
                        {
                            BackgroundWorker worker = (BackgroundWorker)sender;
                            worker.ReportProgress(0, obj);
                        }
                    }

                while(true)
                {
                       
                }


                 */
                log4net.ILog Log = log4net.LogManager.GetLogger("DataSource.StartWatch.watch_bloomberg");

                MktDataService mds = new MktDataService();
                mds.run();
                Log.Info("Started Bloomberg connection....");

                StaticDataService sds = new StaticDataService();
                Log.Info("Started Bloomberg connection....");

                while (true)
                {
                    if (false == WatchedTickers.IsEmpty)
                    {
                        Security thisSecurity = new Security("");
                        if (WatchedTickers.TryDequeue(out thisSecurity))
                        {
                            // 1. Get Static Data ... 
                            sds.MakeRequest(thisSecurity);

                            //2. Add MktDataSubscription
                            mds.addNewSubscription(thisSecurity);
                        }
                    }
                }

                /*                // some how pull oms system and enqueue into MarketDataUpdates
                                int mult = 1;
                                while (true)
                                {
                                    Security update_ticker = new Security("BNP FP")
                                    {

                                        PreviousClose = 4.6,
                                        Last = 4.6 + mult * 0.23
                                    };
                                    RTUpdatesQueue.Enqueue(update_ticker);

                                    System.Threading.Thread.Sleep(1500);
                                    mult++;
                                }
                */
            };

            Task.Factory.StartNew(() =>
                {
                    Parallel.Invoke(watch_bloomberg, watch_oms);
                });
        }
    }
}
