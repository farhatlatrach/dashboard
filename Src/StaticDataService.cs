
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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
using log4net;

namespace Dashboard
{
    class StaticDataService
    {

        public const string LAST_PRICE = "LAST_PRICE";
        public const string PREV_CLOSE = "PX_YEST_CLOSE";
        public const string PREV_ADJ_CLOSE = "PX_DIV_ADJ_CLOSE_1D";
        public const string QUOTED_CRNCY = "CRNCY";
        public const string COUNTRY = "COUNTRY_FULL_NAME";
        public const string SECTOR = "GICS_INDUSTRY_NAME";


        private static readonly Name EXCEPTIONS = new Name("exceptions");
        private static readonly Name FIELD_ID = new Name("fieldId");
        private static readonly Name REASON = new Name("reason");
        private static readonly Name CATEGORY = new Name("category");
        private static readonly Name DESCRIPTION = new Name("description");
        private static readonly Name ERROR_CODE = new Name("errorCode");
        private static readonly Name SOURCE = new Name("source");
        private static readonly Name SECURITY_ERROR = new Name("securityError");
        private static readonly Name MESSAGE = new Name("message");
        private static readonly Name RESPONSE_ERROR = new Name("responseError");
        private static readonly Name SECURITY_DATA = new Name("securityData");
        private static readonly Name FIELD_DATA = new Name("fieldData");
        private static readonly Name FIELD_EXCEPTIONS = new Name("fieldExceptions");
        private static readonly Name ERROR_INFO = new Name("errorInfo");

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SessionOptions d_sessionOptions;
        private Session d_session;
        private DataSet d_bulkData;
        private DataTable d_data;
        private Service refDataService;

        private int d_numberOfReturnedSecurities = 0;

        public StaticDataService()
        {
            //InitializeComponent();
            string serverHost = "localhost";
            int serverPort = 8194;

            // set sesson options
            d_sessionOptions = new SessionOptions();
            d_sessionOptions.ServerHost = serverHost;
            d_sessionOptions.ServerPort = serverPort;

            //            log4net.ILog Log = log4net.LogManager.GetLogger("StaticDataService");
            // initialize UI controls
            //initUI();
        }

        #region methods
        /// <summary>
        /// Initialize form controls
        /// </summary>

        /// <summary>
        /// Create session
        /// </summary>
        /// <returns></returns>
        private bool createSession()
        {
            // create asynchronous session
            d_session = new Session(d_sessionOptions);
            return d_session.Start();
        }

        public bool start()
        {
            if (!createSession())
            {
                Log.Info("Failed to create session...");
                return false;
            }

            if (!d_session.OpenService("//blp/refdata"))
            {
                Log.Info("Failed to open refdata service...");
                return false;
            }

            refDataService = d_session.GetService("//blp/refdata");
            Log.Info("SDS session ready ...");
            return true;
        }

        public void MakeRequest(Security sec)
        {

            // create reference data request
            Request request = refDataService.CreateRequest("ReferenceDataRequest");
            // set request parameters
            Element securities = request.GetElement("securities");
            Element fields = request.GetElement("fields");
            //            Element requestOverrides = request.GetElement("overrides");
            request.Set("returnEids", true);
            // populate security
            securities.AppendValue(sec.BloombergTicker);

            // populate fields
            fields.AppendValue(LAST_PRICE);
            fields.AppendValue(PREV_ADJ_CLOSE);
            fields.AppendValue(PREV_CLOSE);
            fields.AppendValue(QUOTED_CRNCY);
            fields.AppendValue(COUNTRY);

            //sector only for Equities
            if(sec.SecurityType == Security.E_SecurityType.EquityType)
            {
                fields.AppendValue(SECTOR);
            }
            

            //// create correlation id            
            //CorrelationID cID = new CorrelationID(1);
            //d_session.Cancel(cID);
            // send request
            //d_session.SendRequest(request, cID);
            d_session.SendRequest(request, null);
            // Allow UI to update
            //Application.DoEvents();
            // Synchronous mode. Wait for reply before proceeding.
            while (true)
            {
                Event eventObj = d_session.NextEvent();
                Log.Info("Processing data...");
                // process data
                if (eventObj.Type != Event.EventType.RESPONSE && eventObj.Type != Event.EventType.RESPONSE)
                {
                    processMiscEvents(eventObj);
                }
                if (eventObj.Type == Event.EventType.RESPONSE || eventObj.Type == Event.EventType.RESPONSE)
                {
                    processRequestDataEvent(eventObj, sec);
                }
                //processEvent(eventObj, d_session);
                if (eventObj.Type == Event.EventType.RESPONSE)
                {
                    break;
                }
            }
            //setControlStates();
            //toolStripStatusLabel1.Text = "Completed";

        }
        #endregion

        #region Bloomberg API Events
        /// <summary>
        /// Data event
        /// </summary>
        /// <param name="eventObj"></param>
        /// <param name="session"></param>

        private void processRequestDataEvent(Event eventObj, Security sec)
        {
            foreach (Message msg in eventObj)
            {
                // get message correlation id
                //int cId = (int)msg.CorrelationID.Value;
                if (msg.MessageType.Equals(Bloomberglp.Blpapi.Name.GetName("ReferenceDataResponse")))
                {
                    // process security data
                    Element secDataArray = msg.GetElement(SECURITY_DATA);
                    int numberOfSecurities = secDataArray.NumValues;
                    for (int index = 0; index < numberOfSecurities; index++)
                    {
                        Element secData = secDataArray.GetValueAsElement(index);
                        d_numberOfReturnedSecurities++;
                        // get security index
                        // check for field error
                        if (secData.HasElement(FIELD_EXCEPTIONS))
                        {
                            // process error
                            Element error = secData.GetElement(FIELD_EXCEPTIONS);
                            for (int errorIndex = 0; errorIndex < error.NumValues; errorIndex++)
                            {
                                Element errorException = error.GetValueAsElement(errorIndex);
                                string field = errorException.GetElementAsString(FIELD_ID);
                                Element errorInfo = errorException.GetElement(ERROR_INFO);
                                string message = errorInfo.GetElementAsString(MESSAGE);
                                Log.Error(String.Format("Field exception error while processing data {0}", message));
                            }
                        }
                        // check for security error
                        if (secData.HasElement(SECURITY_ERROR))
                        {
                            Element error = secData.GetElement(SECURITY_ERROR);
                            string errorMessage = error.GetElementAsString(MESSAGE);
                            Log.Error(String.Format("Security Error while processing data {0}", errorMessage));
                        }

                        // process data
                        if (secData.HasElement(FIELD_DATA))
                        {
                            Element fieldData = secData.GetElement(FIELD_DATA);
                            int numberOfFields = fieldData.NumElements;
                            Log.Info(string.Format("Ticker: {0}", sec.BloombergTicker));
                            for (int jj = 0; jj < numberOfFields; jj++)
                            {
                                Element item = fieldData.GetElement(jj);
                                Log.Info(string.Format("{0} = {1}", item.Name.ToString(), item.GetValueAsString()));

                                switch (item.Name.ToString())
                                {
                                    case PREV_CLOSE:
                                        sec.PreviousClose = item.GetValueAsFloat64();
                                        break;
                                    case PREV_ADJ_CLOSE:
                                        sec.PreviousAdjClose = item.GetValueAsFloat64();
                                        break;
                                    case QUOTED_CRNCY:
                                        sec.Currency = item.GetValueAsString();
                                        break;
                                    case COUNTRY:
                                        sec.Country = item.GetValueAsString();
                                        break;
                                    case SECTOR:
                                        sec.Sector = item.GetValueAsString();
                                        break;
                                    case LAST_PRICE:
                                        sec.Last = item.GetValueAsFloat64();
                                        break;
                                    default:
                                        Log.Error(string.Format("unknown field name {0}", item.ToString()));
                                        break;
                                }
                            }
                        }

                    }
                }
            }
        }

        
        private void processMiscEvents(Event eventObj)
        {
            foreach (Message msg in eventObj)
            {
                switch (msg.MessageType.ToString())
                {
                    case "SessionStarted":
                        // "Session Started"
                        break;
                    case "SessionTerminated":
                    case "SessionStopped":
                        // "Session Terminated"
                        break;
                    case "ServiceOpened":
                        // "Reference Service Opened"
                        break;
                    case "RequestFailure":
                        Element reason = msg.GetElement(REASON);
                        string message = string.Concat("Error: Source-", reason.GetElementAsString(SOURCE),
                            ", Code-", reason.GetElementAsString(ERROR_CODE), ", category-", reason.GetElementAsString(CATEGORY),
                            ", desc-", reason.GetElementAsString(DESCRIPTION));
                        Log.Error(String.Format("SDS Request failure {0}", message));
                        break;
                    default:
                        Log.Error(String.Format("SDS uknown request issue {0}", msg.MessageType.ToString()));
                        break;
                }
            }
        }
        #endregion
    }

}
