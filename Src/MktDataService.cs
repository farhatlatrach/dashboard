using Event = Bloomberglp.Blpapi.Event;
using Message = Bloomberglp.Blpapi.Message;
using Element = Bloomberglp.Blpapi.Element;
using Name = Bloomberglp.Blpapi.Name;
using Request = Bloomberglp.Blpapi.Request;
using Service = Bloomberglp.Blpapi.Service;
using Session = Bloomberglp.Blpapi.Session;
using SessionOptions = Bloomberglp.Blpapi.SessionOptions;
using EventHandler = Bloomberglp.Blpapi.EventHandler;
using CorrelationID = Bloomberglp.Blpapi.CorrelationID;
using Subscription = Bloomberglp.Blpapi.Subscription;

using TraceLevel = System.Diagnostics.TraceLevel;
using ArrayList = System.Collections.ArrayList;
using Thread = System.Threading.Thread;
using System.Collections.Generic;
using System.IO;
using System;

namespace Dashboard
{
    class MktDataService
    {
        private const string MKTDATA_SVC = "//blp/mktdata";
        private const string AUTH_SVC = "//blp/apiauth";

        private static readonly Name SLOW_CONSUMER_WARNING = Name.GetName("SlowConsumerWarning");
        private static readonly Name SLOW_CONSUMER_WARNING_CLEARED =
                                                               Name.GetName("SlowConsumerWarningCleared");
        private static readonly Name DATA_LOSS = Name.GetName("DataLoss");
        private static readonly Name SUBSCRIPTION_TERMINATED =
                                                               Name.GetName("SubscriptionTerminated");
        private static readonly Name SOURCE = Name.GetName("source");
        private static readonly Name EXCEPTIONS = Name.GetName("exceptions");
        private static readonly Name FIELD_ID = Name.GetName("fieldId");
        private static readonly Name REASON = Name.GetName("reason");
        private static readonly Name ERROR_CODE = Name.GetName("errorCode");
        private static readonly Name CATEGORY = Name.GetName("category");
        private static readonly Name DESCRIPTION = Name.GetName("description");
        private static readonly Name eventTypeName = new Name("MKTDATA_EVENT_TYPE");
        private static readonly Name eventSubTypeName = new Name("MKTDATA_EVENT_SUBTYPE");


        private List<string> d_hosts;
        private int d_port;
        private Session d_session;
        private List<string> d_securities;
        private List<string> d_fields;
        private List<string> d_topics;

        private Dictionary<long, string> d_mapCIDtoTickers;

        private List<Subscription> d_subscriptions;
        private bool d_isSlow;
        private bool d_isStopped;
        private List<Subscription> d_pendingSubscriptions;
        private Dictionary<CorrelationID, object> d_pendingUnsubscribe;
        private object d_lock;
        private string d_service;

        private Boolean d_isSubscribed = false;



        public MktDataService()
        {
            d_hosts = new List<string>();
            d_port = 8194;
            d_securities = new List<string>();
            d_fields = new List<string>();
            d_topics = new List<string>();
            d_mapCIDtoTickers = new Dictionary<long, string>();
            d_subscriptions = new List<Subscription>();
            d_service = "//blp/mktdata";
            d_isSlow = false;
            d_isStopped = false;
            d_pendingSubscriptions = new List<Subscription>();
            d_pendingUnsubscribe = new Dictionary<CorrelationID, object>();
            d_lock = new object();

        }

        private bool createSession()
        {
            if (d_session != null) d_session.Stop();

            //get securities from Model
            d_securities = Model.Instance.getSecurities();

            string authOptions = string.Empty;
            SessionOptions options = new SessionOptions();

            d_session = new Session(options, processEvent);
            System.Console.WriteLine("Starting session...");
            return d_session.Start();
        }



        private void subscribe()
        {
            List<string> subscriptionOptions = new List<string>();
            List<string> fieldList = new List<string>();
            fieldList.Add("LAST_PRICE");
            fieldList.Add("MKTDATA_EVENT_TYPE");
            fieldList.Add("MKTDATA_EVENT_SUBTYPE");
            fieldList.Add("IS_DELAYED_STREAM");

            long cid = 1;
            foreach (var secIter in d_securities)
            {
                subscriptionOptions.Add("interval = 5.0"); // throttle updates every 5 seconds
                string sec = secIter.ToString();
                d_topics.Add(sec);
                d_subscriptions.Add(new Subscription(sec, fieldList, subscriptionOptions, new CorrelationID(cid)));
                d_mapCIDtoTickers.Add(cid, sec);
                cid++;
            }

            d_session.Subscribe(d_subscriptions);
            d_isSubscribed = true;
        }

        public static void startBloomberg()
        {
            
            //   start bloomberg connection
            MktDataService mktDataService = new MktDataService();
            mktDataService.run();
        }

        private void run ()
        {
            // BLPAPI logging
            //            registerCallback(TraceLevel.Off);

            // create session
            if (!createSession())
            {
  //              System.Console.WriteLine("Fail to open session");
                return;
            }



            if (!d_session.OpenService(MKTDATA_SVC))
            {
//                System.Console.Error.WriteLine("Failed to open " + MKTDATA_SVC);
                d_session.Stop();
                return;
            }

//            System.Console.WriteLine("Subscribing...");
            subscribe();

            // wait for enter key to exit application
//            System.Console.Read();

            lock (d_lock)
            {
                d_isStopped = true;
            }
            d_session.Stop();
//            System.Console.WriteLine("Exiting.");
        }

        public void processEvent(Event eventObj, Session session)
        {
            try
            {
                switch (eventObj.Type)
                {
                    case Event.EventType.SUBSCRIPTION_DATA:
                        processSubscriptionDataEvent(eventObj, session);
                        break;
                    case Event.EventType.SUBSCRIPTION_STATUS:
                        processSubscriptionStatus(eventObj, session);
                        break;
                    case Event.EventType.ADMIN:
                        lock (d_lock)
                        {
                            processAdminEvent(eventObj, session);
                        }
                        break;

                    default:
                        processMiscEvents(eventObj, session);
                        break;
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }

        private void processSubscriptionStatus(Event eventObj, Session session)
        {
            System.Console.WriteLine("Processing SUBSCRIPTION_STATUS");
            List<Subscription> subscriptionList = null;
            foreach (Message msg in eventObj)
            {
                CorrelationID cid = msg.CorrelationID;
                string topic = d_topics[(int)cid.Value - 1];
                System.Console.WriteLine(System.DateTime.Now.ToString("s") +
                    ": " + topic + " - " + msg.MessageType);
                System.Console.WriteLine(msg);

                if (msg.MessageType == SUBSCRIPTION_TERMINATED
                            && d_pendingUnsubscribe.Remove(cid))
                {
                    // If this message was due to a previous unsubscribe
                    Subscription subscription = getSubscription(cid);
                    if (d_isSlow)
                    {
                        System.Console.WriteLine("Deferring subscription for topic = {0} because session is slow.", topic);
                        d_pendingSubscriptions.Add(subscription);
                    }
                    else
                    {
                        if (subscriptionList == null)
                        {
                            subscriptionList = new List<Subscription>();
                        }
                        subscriptionList.Add(subscription);
                    }
                }
            }

            if (subscriptionList != null && !d_isStopped)
            {
                session.Subscribe(subscriptionList);
            }
        }

        private void processSubscriptionDataEvent(Event eventObj, Session session)
        {
            //            System.Console.WriteLine("Processing SUBSCRIPTION_DATA");
            foreach (Message msg in eventObj)
            {
                long lCID = (long)msg.CorrelationID.Value;
                //                string topic = d_topics[lCID - 1];

                string sec = "";

                if (d_mapCIDtoTickers.TryGetValue(lCID, out sec))
                {
                    if (msg.HasElement("LAST_PRICE"))
                    {
                        if (!msg.GetElement("LAST_PRICE").IsNull)
                        {
                            Security updated_sec = new Security()
                            {
                                Name = sec,
                                Last = msg.GetElement("LAST_PRIC").GetValueAsFloat64()
                            };

                            DataSource.RTUpdatesQueue.Enqueue(updated_sec);
                        }
                    }
                }
            }
        }

        /*                foreach (Element field in msg.Elements)
                        {
                            if (field.IsNull)
                            {
                                System.Console.WriteLine("\t\t" + field.Name + " is NULL");
                                continue;
                            }

                            if (msg.HasElement("LAST_PRICE"))
                            {
                                element.GetValueAsString()
                                processElement(field);
                            }
                        }*/


        //                System.Console.WriteLine("");


        private void processElement(Element element)
        {
            if (element.IsArray)
            {
                System.Console.WriteLine("\t\t" + element.Name);
                // process array
                int numOfValues = element.NumValues;
                for (int i = 0; i < numOfValues; ++i)
                {
                    // process array data
                    processElement(element.GetValueAsElement(i));
                }
            }
            else if (element.NumElements > 0)
            {
                System.Console.WriteLine("\t\t" + element.Name);
                int numOfElements = element.NumElements;
                for (int i = 0; i < numOfElements; ++i)
                {
                    // process child elements
                    processElement(element.GetElement(i));
                }
            }
            else
            {
                // Assume all values are scalar.
                // update the queue here 



                System.Console.WriteLine("\t\t" + element.Name
                    + " = " + element.GetValueAsString());
            }
        }



        private void processAdminEvent(Event eventObj, Session session)
        {
            System.Console.WriteLine("Processing ADMIN");
            List<CorrelationID> cidsToCancel = null;
            bool previouslySlow = d_isSlow;
            foreach (Message msg in eventObj)
            {
                // An admin event can have more than one messages.
                if (msg.MessageType == SLOW_CONSUMER_WARNING)
                {
                    System.Console.WriteLine(msg);
                    d_isSlow = true;
                }
                else if (msg.MessageType == SLOW_CONSUMER_WARNING_CLEARED)
                {
                    System.Console.WriteLine(msg);
                    d_isSlow = false;
                }
                else if (msg.MessageType == DATA_LOSS)
                {
                    CorrelationID cid = msg.CorrelationID;
                    string topic = d_topics[(int)cid.Value - 1];
                    System.Console.WriteLine("{0}: {1}",
                                             System.DateTime.Now.ToString("s"),
                                             topic);
                    System.Console.WriteLine(msg);
                    if (msg.HasElement(SOURCE))
                    {
                        string sourceStr = msg.GetElementAsString(SOURCE);
                        if (sourceStr.CompareTo("InProc") == 0
                                && !d_pendingUnsubscribe.ContainsKey(cid))
                        {
                            // DataLoss was generated "InProc". This can only happen if
                            // applications are processing events slowly and hence are not
                            // able to keep-up with the incoming events.
                            if (cidsToCancel == null)
                            {
                                cidsToCancel = new List<CorrelationID>();
                            }
                            cidsToCancel.Add(cid);
                            d_pendingUnsubscribe.Add(cid, null);
                        }
                    }
                }
            }

            if (!d_isStopped)
            {
                if (cidsToCancel != null)
                {
                    session.Cancel(cidsToCancel);
                }
                else if ((previouslySlow && !d_isSlow) && d_pendingSubscriptions.Count > 0)
                {
                    // Session was slow but is no longer slow. subscribe to any topics
                    // for which we have previously received SUBSCRIPTION_TERMINATED
                    System.Console.WriteLine("Subscribing to topics - {0}",
                                                                    getTopicsString(d_pendingSubscriptions));
                    session.Subscribe(d_pendingSubscriptions);
                    d_pendingSubscriptions.Clear();
                }
            }
        }

        private void processMiscEvents(Event eventObj, Session session)
        {
            System.Console.WriteLine("Processing " + eventObj.Type);
            foreach (Message msg in eventObj)
            {
                System.Console.WriteLine(System.DateTime.Now.ToString("s") +
                    ": " + msg.MessageType + "\n");
            }
        }

        private Subscription getSubscription(CorrelationID cid)
        {
            foreach (Subscription subscription in d_subscriptions)
            {
                if (subscription.CorrelationID.Equals(cid))
                {
                    return subscription;
                }
            }
            throw new KeyNotFoundException("No subscription found corresponding to cid = " + cid.ToString());
        }

        private string getTopicsString(List<Subscription> list)
        {
            string topics = string.Empty;
            for (int count = 0; count < list.Count; ++count)
            {
                Subscription subscription = list[count];
                if (count != 0)
                {
                    topics += ", ";
                }
                CorrelationID cid = subscription.CorrelationID;
                topics += d_topics[(int)cid.Value - 1];
            }
            return topics;
        }

        /*internal class LoggingCallback : Logging.Callback
        {
            public void OnMessage(long threadId,
                TraceLevel level,
                Datetime dateTime,
                String loggerName,
                String message)
            {
                System.Console.WriteLine(dateTime + "  " + loggerName
                    + " [" + level.ToString() + "] Thread ID = "
                    + threadId + " " + message);
            }
        }

        private void registerCallback(TraceLevel level)
        {
            Logging.RegisterCallback(new LoggingCallback(), level);
        }
        */
    }

}
