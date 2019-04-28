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
                log4net.ILog Log = log4net.LogManager.GetLogger("DataSource.StartWatch.watch_oms");



            };

            //    // watch bloom this action is starting at the start of the program
            Action watch_bloomberg = () =>
            {
                log4net.ILog Log = log4net.LogManager.GetLogger("DataSource.StartWatch.watch_bloomberg");

                MktDataService mds = new MktDataService();
                mds.run();
                Log.Info("Started Bloomberg connection....");

                StaticDataService sds = new StaticDataService();
                sds.start();
                Log.Info("Started Static Data Service connection....");

                while (true)
                {
                    if (false == WatchedTickers.IsEmpty)
                    {
                        Security thisSecurity = new Security("");
                        if (WatchedTickers.TryDequeue(out thisSecurity))
                        {
                            // 1. Get Static Data ... 
                            sds.MakeRequest(thisSecurity);

                            // 2. Add this as a Security Update
                            RTUpdatesQueue.Enqueue(thisSecurity);

                            //3. Add MktDataSubscription
                            mds.addNewSubscription(thisSecurity);
                        }
                    }
                }

            };

            Task.Factory.StartNew(() =>
                {
                    Parallel.Invoke(watch_bloomberg, watch_oms);
                });
        }
    }
}
